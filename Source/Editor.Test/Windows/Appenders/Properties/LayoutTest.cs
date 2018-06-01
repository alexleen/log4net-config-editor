// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using Editor.Descriptors;
using Editor.HistoryManager;
using Editor.Windows;
using Editor.Windows.Appenders.Properties;
using Editor.Windows.PropertyCommon;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.Windows.Appenders.Properties
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
            mSut = new Layout(new ObservableCollection<IProperty>(), mHistoryManager);
        }

        [Test]
        public void Ctor_ShouldInitLayoutsCorrectly()
        {
            CollectionAssert.AreEquivalent(new[] { LayoutDescriptor.Simple, LayoutDescriptor.Pattern }, mSut.Layouts);
        }

        [Test]
        public void Ctor_ShouldInitHistoricalLayoutsCorrectly()
        {
            IEnumerable<string> historicalLayouts = new[] { "layout1", "layout2" };

            mHistoryManager.Get().Returns(historicalLayouts);

            mSut = new Layout(new ObservableCollection<IProperty>(), mHistoryManager);

            CollectionAssert.AreEquivalent(historicalLayouts, mSut.HistoricalLayouts);
        }

        [Test]
        public void Ctor_ShouldInitSelectedLayoutCorrectly()
        {
            Assert.AreEqual(LayoutDescriptor.Simple, mSut.SelectedLayout);
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

        [Test]
        public void Load_ShouldLoadCorrectTypeAndPattern()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\r\n" +
                           "    <layout type=\"log4net.Layout.PatternLayout\">\r\n" +
                           "      <conversionPattern value=\"%date{HH:mm:ss:fff} %message%newline\" />\r\n" +
                           "    </layout>\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(LayoutDescriptor.Pattern, mSut.SelectedLayout);
            Assert.AreEqual("%date{HH:mm:ss:fff} %message%newline", mSut.Pattern);
        }

        [Test]
        public void Load_ShouldNotChangeTypeOrPattern_WhenTypeIsUnknown()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\r\n" +
                           "    <layout>\r\n" +
                           "      <conversionPattern value=\"%date{HH:mm:ss:fff} %message%newline\" />\r\n" +
                           "    </layout>\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            //Verified they're unchanged
            Assert.AreEqual(LayoutDescriptor.Simple, mSut.SelectedLayout);
            Assert.AreEqual(SimplePattern, mSut.Pattern);
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
        public void TryValidate_ShouldShowMessageBox_WhenPatternIsNullOrEmpty(string pattern)
        {
            mSut.Pattern = pattern;

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            mSut.TryValidate(messageBoxService);

            messageBoxService.Received(1).ShowError("A pattern must be assigned to this appender.");
        }

        [TestCase("")]
        [TestCase(null)]
        public void TryValidate_ShouldReturnFalse_WhenPatternIsNullOrEmpty(string pattern)
        {
            mSut.Pattern = pattern;

            Assert.IsFalse(mSut.TryValidate(Substitute.For<IMessageBoxService>()));
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
