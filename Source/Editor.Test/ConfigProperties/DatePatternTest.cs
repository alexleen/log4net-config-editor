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
    public class DatePatternTest
    {
        private DatePattern mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new DatePattern(new ReadOnlyCollection<IProperty>(new List<IProperty>()));
        }

        [Test]
        public void Load_ShouldSetCorrectValue()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\r\n" +
                           "    <datePattern value=\"yyyyMMdd\" />\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual("yyyyMMdd", mSut.Value);
        }

        [Test]
        public void Load_ShouldNotSetValue_WhenElementDoesNotExist()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.IsNull(mSut.Value);
        }

        [Test]
        public void Load_ShouldNotSetValue_WhenAttributeValueDoesNotExist()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\r\n" +
                           "    <datePattern />\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.IsNull(mSut.Value);
        }

        [TestCase(null)]
        [TestCase("")]
        public void TryValidate_ShouldShowMessageBoxAndReturnsFalse_WhenValueIsNullOrEmpty(string value)
        {
            mSut.Value = value;

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsFalse(mSut.TryValidate(messageBoxService));

            messageBoxService.Received(1).ShowError("A valid date pattern must be assigned.");
        }

        [Test]
        public void TryValidate_ShouldNotShowMessageBox_WhenValueIsNotNullOrEmpty()
        {
            mSut.Value = "yyyyMMdd";

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            mSut.TryValidate(messageBoxService);

            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }

        [Test]
        public void TryValidate_ShouldReturnTrue_WhenValueIsNotNullOrEmpty()
        {
            mSut.Value = "yyyyMMdd";

            Assert.IsTrue(mSut.TryValidate(Substitute.For<IMessageBoxService>()));
        }

        [Test]
        public void Save_ShouldCreateAndAppendCorrectElement()
        {
            const string value = "yyyyMMdd";
            mSut.Value = value;
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Save(xmlDoc, appender);

            XmlElement datePattern = appender["datePattern"];
            Assert.IsNotNull(datePattern);
            Assert.AreEqual(value, datePattern.Attributes["value"].Value);
        }
    }
}
