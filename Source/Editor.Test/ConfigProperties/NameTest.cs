// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private Name mSut;
        private XmlNode mLog4NetNode;
        private XmlDocument mXmlDoc;
        private XmlNode mOriginalAppender;

        [SetUp]
        public void SetUp()
        {
            mXmlDoc = new XmlDocument();
            mLog4NetNode = mXmlDoc.CreateElement("log4net");
            mOriginalAppender = mXmlDoc.CreateElementWithAttribute("appender", "name", "appName");
            mOriginalAppender.AppendTo(mLog4NetNode);

            IElementConfiguration appenderConfiguration = Substitute.For<IElementConfiguration>();
            appenderConfiguration.ConfigXml.Returns(mXmlDoc);
            appenderConfiguration.Log4NetNode.Returns(mLog4NetNode);
            appenderConfiguration.OriginalNode.Returns(mOriginalAppender);

            mSut = new Name(new ReadOnlyCollection<IProperty>(new List<IProperty>()), appenderConfiguration);
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
        public void TryValidate_ShouldNotShowUnassignedMessageBox_WhenValueIsNotNullOrEmpty_AndReturnTrue()
        {
            mSut.Value = "name";

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsTrue(mSut.TryValidate(messageBoxService));
            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }

        [Test]
        public void TryValidate_ShouldNotShowCollisionMessageBox_WhenNoAppenderNamesCollide()
        {
            mSut.Value = "appName";
            mXmlDoc.CreateElementWithAttribute("appender", "name", "otherName").AppendTo(mLog4NetNode);

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsTrue(mSut.TryValidate(messageBoxService));
            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
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
        public void TryValidate_ShouldShowCollisionMessageBox_WhenAppenderNameCollides()
        {
            mSut.Value = "appName";
            mXmlDoc.CreateElementWithAttribute("appender", "name", mSut.Value).AppendTo(mLog4NetNode);

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsFalse(mSut.TryValidate(messageBoxService));
            messageBoxService.Received(1).ShowError("Name must be unique.");
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
    }
}
