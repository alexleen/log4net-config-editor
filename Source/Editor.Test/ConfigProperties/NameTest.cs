// Copyright © 2018 Alex Leendertsen

using System.Xml;
using Editor.ConfigProperties;
using Editor.Interfaces;
using Editor.Utilities;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class NameTest
    {
        [SetUp]
        public void SetUp()
        {
            mXmlDoc = new XmlDocument();
            mLog4NetNode = mXmlDoc.CreateElement("log4net");
            mOriginalAppender = mXmlDoc.CreateElementWithAttribute("appender", "name", OriginalName);
            mOriginalAppender.AppendTo(mLog4NetNode);

            mAppenderConfiguration = Substitute.For<IElementConfiguration>();
            mAppenderConfiguration.ConfigXml.Returns(mXmlDoc);
            mAppenderConfiguration.Log4NetNode.Returns(mLog4NetNode);
            mAppenderConfiguration.OriginalNode.Returns(mOriginalAppender);

            mSut = new Name(mAppenderConfiguration);
        }

        private const string OriginalName = "appName";
        private Name mSut;
        private XmlNode mLog4NetNode;
        private XmlDocument mXmlDoc;
        private XmlNode mOriginalAppender;
        private IElementConfiguration mAppenderConfiguration;

        [TestCase("name=\"\"")]
        [TestCase("")]
        public void Load_ShouldNotLoadName(string name)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml($"<appender {name} type=\"log4net.Appender.ColoredConsoleAppender\">\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.IsNull(mSut.Value);
        }

        [TestCase("")]
        [TestCase(null)]
        public void TryValidate_ShouldShowUnassignedMessageBox_WhenValueIsNullOrEmpty_AndReturnFalse(string value)
        {
            mSut.Value = value;

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsFalse(mSut.TryValidate(messageBoxService));
            messageBoxService.Received(1).ShowError("A name must be assigned to this appender.");
        }

        [Test]
        public void Changed_ShouldBeFalse_WhenValueDoesMatchOriginal()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml($"<appender name=\"{OriginalName}\" type=\"log4net.Appender.ColoredConsoleAppender\">\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.IsFalse(mSut.Changed);
        }

        [Test]
        public void Changed_ShouldBeNull_WhenNoOriginalNameExists()
        {
            mAppenderConfiguration.OriginalNode.Returns((XmlNode)null);

            Assert.IsNull(mSut.Changed);
        }

        [Test]
        public void Changed_ShouldBeTrue_WhenValueDoesNotMatchOriginal()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml($"<appender name=\"{OriginalName}\" type=\"log4net.Appender.ColoredConsoleAppender\">\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            mSut.Value = "someOtherName";

            Assert.IsTrue(mSut.Changed);
        }

        [Test]
        public void IsFocused_ShouldBeTrue()
        {
            Assert.IsTrue(mSut.IsFocused);
        }

        [Test]
        public void Load_ShouldLoadCorrectName()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender name=\"ColoredConsoleAppender\" type=\"log4net.Appender.ColoredConsoleAppender\">\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual("ColoredConsoleAppender", mSut.Value);
        }

        [Test]
        public void OriginalName_ShouldBeNull_WhenNoOriginalNameAttrExists()
        {
            mAppenderConfiguration.OriginalNode.Attributes.RemoveNamedItem("name");

            Assert.IsNull(mSut.OriginalName);
        }

        [Test]
        public void OriginalName_ShouldBeNull_WhenNoOriginalNodeExists()
        {
            mAppenderConfiguration.OriginalNode.Returns((XmlNode)null);

            Assert.IsNull(mSut.OriginalName);
        }

        [Test]
        public void OriginalName_ShouldMatchOriginalName_WhenOriginalNameExists()
        {
            Assert.AreEqual(OriginalName, mSut.OriginalName);
        }

        [Test]
        public void Save_ShouldSaveNameToAttribute()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            const string nameValue = "name";
            mSut.Value = nameValue;

            mSut.Save(xmlDoc, appender);

            Assert.AreEqual(nameValue, appender.Attributes["name"].Value);
        }

        [Test]
        public void TryValidate_ShouldNotShowCollisionMessageBox_WhenAppenderNameCollides_ButIsSameAppender()
        {
            mSut.Load(mOriginalAppender);

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsTrue(mSut.TryValidate(messageBoxService));
            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }

        [Test]
        public void TryValidate_ShouldNotShowCollisionMessageBox_WhenNoAppenderNamesCollide()
        {
            mSut.Value = OriginalName;
            mXmlDoc.CreateElementWithAttribute("appender", "name", "otherName").AppendTo(mLog4NetNode);

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsTrue(mSut.TryValidate(messageBoxService));
            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }

        [Test]
        public void TryValidate_ShouldNotShowCollisionMessageBox_WhenOtherAppenderHasNoName()
        {
            mSut.Value = "appName";
            mXmlDoc.CreateElement("appender").AppendTo(mLog4NetNode);

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsTrue(mSut.TryValidate(messageBoxService));
            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }

        [Test]
        public void TryValidate_ShouldNotShowUnassignedMessageBox_WhenValueIsNotNullOrEmpty_AndReturnTrue()
        {
            mSut.Value = "name";

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsTrue(mSut.TryValidate(messageBoxService));
            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }

        [Test]
        public void TryValidate_ShouldShowCollisionMessageBox_WhenAppenderNameCollides()
        {
            mSut.Value = OriginalName;
            mXmlDoc.CreateElementWithAttribute("appender", "name", mSut.Value).AppendTo(mLog4NetNode);

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsFalse(mSut.TryValidate(messageBoxService));
            messageBoxService.Received(1).ShowError("Name must be unique.");
        }
    }
}
