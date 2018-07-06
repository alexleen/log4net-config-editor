// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class LogNameTest
    {
        private LogName mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new LogName(new ReadOnlyCollection<IProperty>(new List<IProperty>()));
        }

        [Test]
        public void Load_ShouldSetCorrectValue()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender name=\"WindowsEventLog\" type=\"log4net.Appender.EventLogAppender\">\r\n" +
                           "    <param name=\"LogName\" value=\"SEL-5051\" />\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual("SEL-5051", mSut.Value);
        }

        [Test]
        public void Load_ShouldNotSetValue_WhenElementDoesNotExist()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender name=\"WindowsEventLog\" type=\"log4net.Appender.EventLogAppender\">\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.IsNull(mSut.Value);
        }

        [Test]
        public void Load_ShouldNotSetValue_WhenAttributeValueDoesNotExist()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender name=\"WindowsEventLog\" type=\"log4net.Appender.EventLogAppender\">\r\n" +
                           "    <param name=\"LogName\" />\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.IsNull(mSut.Value);
        }

        [Test]
        public void Save_ShouldCreateAndAppendCorrectElement()
        {
            const string value = "SEL-5051";
            mSut.Value = value;
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Save(xmlDoc, appender);

            XmlElement param = appender["param"];
            Assert.IsNotNull(param);
            Assert.AreEqual("LogName", param.Attributes["name"].Value);
            Assert.AreEqual(value, param.Attributes["value"].Value);
        }

        [Test]
        public void TryValidate_ShouldShowMessageBox_WhenValueIsNullOrEmpty()
        {
            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            mSut.TryValidate(messageBoxService);

            messageBoxService.Received(1).ShowError("A log name must be assigned to this appender.");
        }

        [Test]
        public void TryValidate_ShouldReturnFalse_WhenValueIsNullOrEmpty()
        {
            Assert.IsFalse(mSut.TryValidate(Substitute.For<IMessageBoxService>()));
        }

        [Test]
        public void TryValidate_ShouldNotShowMessageBox_WhenValueIsNotNullOrEmpty()
        {
            mSut.Value = "value";

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            mSut.TryValidate(messageBoxService);

            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }

        [Test]
        public void TryValidate_ShouldReturnTrue_WhenValueIsNotNullOrEmpty()
        {
            mSut.Value = "value";

            Assert.IsTrue(mSut.TryValidate(Substitute.For<IMessageBoxService>()));
        }
    }
}
