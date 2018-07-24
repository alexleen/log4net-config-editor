// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Enums;
using Editor.HistoryManager;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class LayoutTest
    {
        private const string SimplePattern = "%level - %message%newline";
        private const string LayoutName = "layout";
        private const string ConversionPatternName = "conversionPattern";
        private Layout mSut;
        private IHistoryManager mHistoryManager;

        [SetUp]
        public void SetUp()
        {
            mHistoryManager = Substitute.For<IHistoryManager>();
            mSut = new Layout(new ReadOnlyCollection<IProperty>(new List<IProperty>()), mHistoryManager);
        }

        [Test]
        public void Ctor_ShouldInitLayoutsCorrectly_WhenRequired()
        {
            CollectionAssert.AreEquivalent(new[] { LayoutDescriptor.Simple, LayoutDescriptor.Pattern }, mSut.Layouts);
        }

        [Test]
        public void Ctor_ShouldInitLayoutsCorrectly_WhenNotRequired()
        {
            mSut = new Layout(new ReadOnlyCollection<IProperty>(new List<IProperty>()), mHistoryManager, false);

            CollectionAssert.AreEquivalent(new[] { LayoutDescriptor.None, LayoutDescriptor.Simple, LayoutDescriptor.Pattern }, mSut.Layouts);
        }

        [Test]
        public void Ctor_ShouldInitHistoricalLayoutsCorrectly()
        {
            IEnumerable<string> historicalLayouts = new[] { "layout1", "layout2" };

            mHistoryManager.Get().Returns(historicalLayouts);

            mSut = new Layout(new ReadOnlyCollection<IProperty>(new List<IProperty>()), mHistoryManager);

            CollectionAssert.AreEquivalent(historicalLayouts, mSut.HistoricalLayouts);
        }

        [Test]
        public void Ctor_ShouldInitSelectedLayoutCorrectly_WhenRequired()
        {
            Assert.AreEqual(LayoutDescriptor.Simple, mSut.SelectedLayout);
        }

        [Test]
        public void Ctor_ShouldInitSelectedLayoutCorrectly_WhenNotRequired()
        {
            mSut = new Layout(new ReadOnlyCollection<IProperty>(new List<IProperty>()), mHistoryManager, false);

            Assert.AreEqual(LayoutDescriptor.None, mSut.SelectedLayout);
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
        public void SelectedLayout_ShouldSetSimplePattern_WhenSetToSimple()
        {
            //SelectedLayout is set to Simple in ctor
            Assert.AreEqual(SimplePattern, mSut.Pattern);
        }

        [Test]
        public void SelectedLayout_ShouldSetSimplePattern_WhenNoOriginalPatternIsPresent()
        {
            mSut.SelectedLayout = LayoutDescriptor.Pattern;
            Assert.AreEqual(SimplePattern, mSut.Pattern);
        }

        [Test]
        public void SelectedLayout_ShouldSetEmptyPattern_WhenSetToNone()
        {
            mSut.SelectedLayout = LayoutDescriptor.None;
            Assert.AreEqual(string.Empty, mSut.Pattern);
        }

        [Test]
        public void SelectedLayout_ShouldSetOriginalPattern_WhenNotSimpleAndOriginalExists()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\r\n" +
                           "    <layout type=\"log4net.Layout.PatternLayout\">\r\n" +
                           "      <conversionPattern value=\"%date{HH:mm:ss:fff} %message%newline\" />\r\n" +
                           "    </layout>\r\n" +
                           "</appender>");

            //Loading the XML will set selected layout to Pattern and save the original pattern
            mSut.Load(xmlDoc.FirstChild);

            //Set to Simple to clear pattern
            mSut.SelectedLayout = LayoutDescriptor.Simple;

            //Set back to Pattern to (hopefully) restore original
            mSut.SelectedLayout = LayoutDescriptor.Pattern;

            Assert.AreEqual("%date{HH:mm:ss:fff} %message%newline", mSut.Pattern);
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
        public void Pattern_ShouldNotFirePropChange_WhenValueHasNotChanged()
        {
            bool fired = false;
            mSut.PropertyChanged += (sender, args) => { fired = true; };
            mSut.Pattern = mSut.Pattern;

            Assert.IsFalse(fired);
        }

        [Test]
        public void Pattern_ShouldFirePropChange_WhenValueHasChanged()
        {
            bool fired = false;
            mSut.PropertyChanged += (sender, args) => { fired = true; };
            mSut.Pattern = " ";

            Assert.IsTrue(fired);
        }

        //Required = false
        [TestCase(null, false, LayoutType.None, "")]
        [TestCase("", false, LayoutType.None, "")]
        [TestCase("<lay>\r\n" +
                  "</lay>\r\n", false, LayoutType.None, "")]
        [TestCase("<layout>\r\n" +
                  "</layout>\r\n", false, LayoutType.None, "")]
        [TestCase("<layout>\r\n" +
                  "    <conversionPattern value=\"%date{HH:mm:ss:fff} %message%newline\" />\r\n" +
                  "</layout>\r\n", false, LayoutType.None, "")]
        [TestCase("<layout type=\"log4net.Layout.PatternLayout\">\r\n" +
                  "    <conversionPattern value=\"%date{HH:mm:ss:fff} %message%newline\" />\r\n" +
                  "</layout>\r\n", false, LayoutType.Pattern, "%date{HH:mm:ss:fff} %message%newline")]
        //Required = true
        [TestCase(null, true, LayoutType.Simple, SimplePattern)]
        [TestCase("", true, LayoutType.Simple, SimplePattern)]
        [TestCase("<lay>\r\n" +
                  "</lay>\r\n", true, LayoutType.Simple, SimplePattern)]
        [TestCase("<layout>\r\n" +
                  "</layout>\r\n", true, LayoutType.Simple, SimplePattern)]
        [TestCase("<layout>\r\n" +
                  "    <conversionPattern value=\"%date{HH:mm:ss:fff} %message%newline\" />\r\n" +
                  "</layout>\r\n", true, LayoutType.Simple, SimplePattern)]
        [TestCase("<layout type=\"log4net.Layout.PatternLayout\">\r\n" +
                  "    <conversionPattern value=\"%date{HH:mm:ss:fff} %message%newline\" />\r\n" +
                  "</layout>\r\n", true, LayoutType.Pattern, "%date{HH:mm:ss:fff} %message%newline")]
        public void Load_ShouldLoadCorrectTypeAndPattern(string layoutXml, bool required, LayoutType expectedLayout, string expectedPattern)
        {
            mSut = new Layout(new ReadOnlyCollection<IProperty>(new List<IProperty>()), mHistoryManager, required);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\r\n" +
                           $"    {layoutXml}" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

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

            mSut.Load(xmlDoc.FirstChild);

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

        [Test]
        public void Save_ShouldCreateAndAppendCorrectElement()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Save(xmlDoc, appender);

            XmlElement layout = appender[LayoutName];
            Assert.IsNotNull(layout);
            Assert.AreEqual(LayoutDescriptor.Simple.TypeNamespace, layout.Attributes["type"].Value);
        }

        [Test]
        public void Save_ShouldNotCreateConversionPatternElement_WhenSimple()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Save(xmlDoc, appender);

            Assert.IsNull(appender[LayoutName][ConversionPatternName]);
        }

        [Test]
        public void Save_ShouldCreateConversionPatternElement_WhenNotSimple()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.SelectedLayout = LayoutDescriptor.Pattern;
            mSut.Pattern = "%date{HH:mm:ss:fff} %message%newline";
            mSut.Save(xmlDoc, appender);

            Assert.IsNotNull(appender[LayoutName][ConversionPatternName]);
            Assert.AreEqual(mSut.Pattern, appender[LayoutName][ConversionPatternName].Attributes["value"].Value);
        }

        [Test]
        public void Save_ShouldNotSaveLayout_WhenNoneIsSelected()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.SelectedLayout = LayoutDescriptor.None;
            mSut.Save(xmlDoc, appender);

            CollectionAssert.IsEmpty(appender.ChildNodes);
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

            mSut.Load(xmlDoc.FirstChild);

            mSut.Pattern = "%message%newline";

            mSut.Save(xmlDoc, xmlDoc.CreateElement("appender"));

            mHistoryManager.Received(1).Save(mSut.Pattern);
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

            mSut.Load(xmlDoc.FirstChild);

            mSut.Save(xmlDoc, xmlDoc.CreateElement("appender"));

            mHistoryManager.DidNotReceive().Save(Arg.Any<string>());
        }
    }
}
