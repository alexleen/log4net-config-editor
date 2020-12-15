// Copyright © 2020 Alex Leendertsen

using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Enums;
using Editor.HistoryManager;
using Editor.Interfaces;
using Editor.Utilities;
using Editor.XML;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class LayoutTest
    {
        [SetUp]
        public void SetUp()
        {
            mHistoryManager = Substitute.For<IHistoryManager>();
            mHistoryManagerFactory = Substitute.For<IHistoryManagerFactory>();
            mHistoryManagerFactory.CreatePatternsHistoryManager().Returns(mHistoryManager);
            mSut = new Layout(mHistoryManagerFactory);
        }

        private const string SimplePattern = "%level - %message%newline";
        private const string LayoutName = "layout";
        private const string ConversionPatternName = "conversionPattern";
        private Layout mSut;
        private IHistoryManager mHistoryManager;
        private IHistoryManagerFactory mHistoryManagerFactory;

        //Required = false
        [TestCase(null, false, LayoutType.None, "")]
        [TestCase("", false, LayoutType.None, "")]
        [TestCase("<lay>\r\n</lay>\r\n", false, LayoutType.None, "")]
        [TestCase("<layout>\r\n</layout>\r\n", false, LayoutType.None, "")]
        [TestCase("<layout>\r\n" +
                  "    <conversionPattern value=\"%date{HH:mm:ss:fff} %message%newline\" />\r\n" +
                  "</layout>\r\n",
                  false,
                  LayoutType.None,
                  "")]
        [TestCase("<layout type=\"log4net.Layout.PatternLayout\">\r\n" +
                  "    <conversionPattern value=\"%date{HH:mm:ss:fff} %message%newline\" />\r\n" +
                  "</layout>\r\n",
                  false,
                  LayoutType.Pattern,
                  "%date{HH:mm:ss:fff} %message%newline")]
        //Required = true
        [TestCase(null, true, LayoutType.Simple, SimplePattern)]
        [TestCase("", true, LayoutType.Simple, SimplePattern)]
        [TestCase("<lay>\r\n</lay>\r\n", true, LayoutType.Simple, SimplePattern)]
        [TestCase("<layout>\r\n</layout>\r\n", true, LayoutType.Simple, SimplePattern)]
        [TestCase("<layout>\r\n" +
                  "    <conversionPattern value=\"%date{HH:mm:ss:fff} %message%newline\" />\r\n" +
                  "</layout>\r\n",
                  true,
                  LayoutType.Simple,
                  SimplePattern)]
        [TestCase("<layout type=\"log4net.Layout.PatternLayout\">\r\n" +
                  "    <conversionPattern value=\"%date{HH:mm:ss:fff} %message%newline\" />\r\n" +
                  "</layout>\r\n",
                  true,
                  LayoutType.Pattern,
                  "%date{HH:mm:ss:fff} %message%newline")]
        public void Load_ShouldLoadCorrectTypeAndPattern(string layoutXml, bool required, LayoutType expectedLayout, string expectedPattern)
        {
            mSut = new Layout(mHistoryManagerFactory, required);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\r\n" +
                           $"    {layoutXml}" +
                           "</appender>");

            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.OriginalNode.Returns(xmlDoc.FirstChild);
            mSut.Load(config);

            //Verified they're unchanged
            Assert.AreEqual(expectedLayout, mSut.SelectedLayout.Type);
            Assert.AreEqual(expectedPattern, mSut.Pattern);
        }

        [TestCase("<conversionPattern value=\"\" />\r\n")]
        [TestCase("<conversionPattern />\r\n")]
        [TestCase("")]
        [TestCase(null)]
        public void Load_ShouldNotChangePattern_WhenPatternIsUnknown(string conversionPattern)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\r\n" +
                           "    <layout type=\"log4net.Layout.PatternLayout\">\r\n" +
                           $"      {conversionPattern}" +
                           "    </layout>\r\n" +
                           "</appender>");

            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.OriginalNode.Returns(xmlDoc.FirstChild);
            mSut.Load(config);

            //Verified it's unchanged
            Assert.AreEqual(SimplePattern, mSut.Pattern);
        }

        [TestCase("")]
        [TestCase(null)]
        public void TryValidate_ShouldShowMessageBox_WhenPatternIsNullOrEmpty_WhenRequired(string pattern)
        {
            mSut.Pattern = pattern;

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            mSut.TryValidate(messageBoxService);

            messageBoxService.Received(1).ShowError("A pattern must be assigned to this appender.");
        }

        [TestCase("")]
        [TestCase(null)]
        public void TryValidate_ShouldReturnFalse_WhenPatternIsNullOrEmpty_WhenRequired(string pattern)
        {
            mSut.Pattern = pattern;

            Assert.IsFalse(mSut.TryValidate(Substitute.For<IMessageBoxService>()));
        }

        [TestCase("")]
        [TestCase(null)]
        public void TryValidate_ShouldReturnTrue_WhenPatternIsNullOrEmpty_WhenNoneIsSelected(string pattern)
        {
            mSut.SelectedLayout = LayoutDescriptor.None;
            mSut.Pattern = pattern;

            Assert.IsTrue(mSut.TryValidate(Substitute.For<IMessageBoxService>()));
        }

        [Test]
        public void Ctor_ShouldInitHistoricalLayoutsCorrectly()
        {
            IEnumerable<string> historicalLayouts = new[] { "layout1", "layout2" };

            mHistoryManager.Get().Returns(historicalLayouts);

            mSut = new Layout(mHistoryManagerFactory);

            CollectionAssert.AreEquivalent(historicalLayouts, mSut.HistoricalLayouts);
        }

        [Test]
        public void Ctor_ShouldInitLayoutsCorrectly_WhenNotRequired()
        {
            mSut = new Layout(mHistoryManagerFactory, false);

            CollectionAssert.AreEquivalent(new[] { LayoutDescriptor.None, LayoutDescriptor.Simple, LayoutDescriptor.Pattern }, mSut.Layouts);
        }

        [Test]
        public void Ctor_ShouldInitLayoutsCorrectly_WhenRequired()
        {
            CollectionAssert.AreEquivalent(new[] { LayoutDescriptor.Simple, LayoutDescriptor.Pattern }, mSut.Layouts);
        }

        [Test]
        public void Ctor_ShouldInitSelectedLayoutCorrectly_WhenNotRequired()
        {
            mSut = new Layout(mHistoryManagerFactory, false);

            Assert.AreEqual(LayoutDescriptor.None, mSut.SelectedLayout);
        }

        [Test]
        public void Ctor_ShouldInitSelectedLayoutCorrectly_WhenRequired()
        {
            Assert.AreEqual(LayoutDescriptor.Simple, mSut.SelectedLayout);
        }

        [Test]
        public void Pattern_ShouldFirePropChange_WhenValueHasChanged()
        {
            bool fired = false;
            mSut.PropertyChanged += (sender, args) => { fired = true; };
            mSut.Pattern = " ";

            Assert.IsTrue(fired);
        }

        [Test]
        public void Pattern_ShouldNotFirePropChange_WhenValueHasNotChanged()
        {
            bool fired = false;
            mSut.PropertyChanged += (sender, args) => { fired = true; };
            mSut.Pattern = mSut.Pattern;

            Assert.IsFalse(fired);
        }

        [Test]
        public void Save_ShouldCreateAndAppendCorrectElement_WhenSimple()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            mSut.Save(config);

            config.Received(1).SaveHierarchical(new Element(LayoutName, new[] { (Log4NetXmlConstants.Type, "log4net.Layout.SimpleLayout") }));
        }

        [Test]
        public void Save_ShouldCreateConversionPatternElement_WhenNotSimple()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.SelectedLayout = LayoutDescriptor.Pattern;
            mSut.Pattern = "%date{HH:mm:ss:fff} %message%newline";

            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            mSut.Save(config);

            config.Received(1).SaveHierarchical(new Element(LayoutName, new[] { (Log4NetXmlConstants.Type, "log4net.Layout.PatternLayout") }), new Element(Log4NetXmlConstants.ConversionPattern, new[] { (Log4NetXmlConstants.Value, mSut.Pattern) }));
        }

        [Test]
        public void Save_ShouldNotSaveLayout_WhenNoneIsSelected()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.SelectedLayout = LayoutDescriptor.None;

            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            mSut.Save(config);

            config.DidNotReceiveWithAnyArgs().Save();
        }

        [Test]
        public void Save_ShouldNotSavePattern_WhenEqualToOriginal()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\r\n" +
                           "    <layout type=\"log4net.Layout.PatternLayout\">\r\n" +
                           "      <conversionPattern value=\"%date{HH:mm:ss:fff} %message%newline\" />\r\n" +
                           "    </layout>\r\n" +
                           "</appender>");

            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.OriginalNode.Returns(xmlDoc.FirstChild);
            mSut.Load(config);

            mSut.Save(config);

            mHistoryManager.DidNotReceive().Save(Arg.Any<string>());
        }

        [Test]
        public void Save_ShouldSavePattern_WhenNotEqualToOriginal()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\r\n" +
                           "    <layout type=\"log4net.Layout.PatternLayout\">\r\n" +
                           "      <conversionPattern value=\"%date{HH:mm:ss:fff} %message%newline\" />\r\n" +
                           "    </layout>\r\n" +
                           "</appender>");

            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.OriginalNode.Returns(xmlDoc.FirstChild);
            mSut.Load(config);

            mSut.Pattern = "%message%newline";

            mSut.Save(config);

            mHistoryManager.Received(1).Save(mSut.Pattern);
        }

        [Test]
        public void SelectedLayout_ShouldBeEmptyPattern_WhenSetToNone()
        {
            mSut.SelectedLayout = LayoutDescriptor.None;
            Assert.AreEqual(string.Empty, mSut.Pattern);
        }

        [Test]
        public void SelectedLayout_ShouldBeLatestHistoricalPattern_WhenPatternAndNoOriginalExists()
        {
            IEnumerable<string> historicalLayouts = new[] { "layout1", "layout2" };

            mHistoryManager.Get().Returns(historicalLayouts);

            mSut = new Layout(mHistoryManagerFactory);

            mSut.SelectedLayout = LayoutDescriptor.Pattern;

            Assert.AreEqual(historicalLayouts.First(), mSut.Pattern);
        }

        [Test]
        public void SelectedLayout_ShouldBeOriginalPattern_WhenNotSimpleAndOriginalExists()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\r\n" +
                           "    <layout type=\"log4net.Layout.PatternLayout\">\r\n" +
                           "      <conversionPattern value=\"%date{HH:mm:ss:fff} %message%newline\" />\r\n" +
                           "    </layout>\r\n" +
                           "</appender>");

            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.OriginalNode.Returns(xmlDoc.FirstChild);

            //Loading the XML will set selected layout to Pattern and save the original pattern
            mSut.Load(config);

            //Set to Simple to clear pattern
            mSut.SelectedLayout = LayoutDescriptor.Simple;

            //Set back to Pattern to (hopefully) restore original
            mSut.SelectedLayout = LayoutDescriptor.Pattern;

            Assert.AreEqual("%date{HH:mm:ss:fff} %message%newline", mSut.Pattern);
        }

        [Test]
        public void SelectedLayout_ShouldBeSimple_WhenPatternAndNoOriginalExistsAndNoHistorical()
        {
            IEnumerable<string> historicalLayouts = Enumerable.Empty<string>();

            mHistoryManager.Get().Returns(historicalLayouts);

            mSut = new Layout(mHistoryManagerFactory);

            mSut.SelectedLayout = LayoutDescriptor.Pattern;

            Assert.AreEqual(SimplePattern, mSut.Pattern);
        }

        [Test]
        public void SelectedLayout_ShouldBeSimplePattern_WhenSetToSimple()
        {
            //SelectedLayout is set to Simple in ctor
            Assert.AreEqual(SimplePattern, mSut.Pattern);
        }

        [Test]
        public void SelectedLayout_ShouldFirePropChange_WhenValueHasChanged()
        {
            bool fired = false;
            mSut.PropertyChanged += (sender, args) => { fired = true; };
            mSut.SelectedLayout = LayoutDescriptor.Pattern;

            Assert.IsTrue(fired);
        }

        [Test]
        public void SelectedLayout_ShouldNotFirePropChange_WhenValueHasNotChanged()
        {
            bool fired = false;
            mSut.PropertyChanged += (sender, args) => { fired = true; };
            mSut.SelectedLayout = mSut.SelectedLayout;

            Assert.IsFalse(fired);
        }

        [Test]
        public void SelectedLayout_ShouldSBeSimplePattern_WhenNoOriginalPatternIsPresent()
        {
            mSut.SelectedLayout = LayoutDescriptor.Pattern;
            Assert.AreEqual(SimplePattern, mSut.Pattern);
        }

        [Test]
        public void TryValidate_ShouldNotShowMessageBox_WhenPatternIsNotNullOrEmpty()
        {
            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            mSut.TryValidate(messageBoxService);

            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }

        [Test]
        public void TryValidate_ShouldReturnTrue_WhenPatternIsNotNullOrEmpty()
        {
            Assert.IsTrue(mSut.TryValidate(Substitute.For<IMessageBoxService>()));
        }
    }
}
