// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Xml;
using Editor.Windows;
using Editor.Windows.Appenders.Properties;
using Editor.Windows.PropertyCommon;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.Windows.Appenders.Properties
{
    [TestFixture]
    public class BufferSizeTest
    {
        private const string DefaultBufferSize = "1000";
        private BufferSize mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new BufferSize(new ObservableCollection<IProperty>());
        }

        [Test]
        public void Value_ShouldBeInitializedToDefault()
        {
            Assert.AreEqual(DefaultBufferSize, mSut.Value);
        }

        [Test]
        public void Load_ShouldSetCorrectValue()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\r\n" +
                           "    <bufferSize value=\"10000\" />\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual("10000", mSut.Value);
        }

        [Test]
        public void Load_ShouldSetDefaultValue_WhenElementDoesNotExist()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(DefaultBufferSize, mSut.Value);
        }

        [Test]
        public void Load_ShouldSetDefaultValue_WhenAttributeValueDoesNotExist()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\r\n" +
                           "    <bufferSize />\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(DefaultBufferSize, mSut.Value);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("string")]
        public void Load_ShouldSetDefaultValue_WhenAttributeValueIsNotAnInt(string value)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\r\n" +
                           $"    <bufferSize value=\"{value}\" />\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(DefaultBufferSize, mSut.Value);
        }

        [Test]
        public void Save_ShouldCreateAndAppendCorrectElement_WhenNotDefault()
        {
            const string value = "10000";
            mSut.Value = value;
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Save(xmlDoc, appender);

            XmlElement bufferSize = appender["bufferSize"];
            Assert.IsNotNull(bufferSize);
            Assert.AreEqual(value, bufferSize.Attributes["value"].Value);
        }

        [Test]
        public void Save_ShouldNotCreateAndAppendCorrectElement_WhenDefault()
        {
            mSut.Value = DefaultBufferSize;
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Save(xmlDoc, appender);

            XmlElement bufferSize = appender["bufferSize"];
            Assert.IsNull(bufferSize);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("string")]
        public void TryValidate_ShouldShowMessageBoxAndReturnsFalse_WhenValueIsNotAnInt(string value)
        {
            mSut.Value = value;

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsFalse(mSut.TryValidate(messageBoxService));

            messageBoxService.Received(1).ShowError("Buffer size must be a valid integer.");
        }

        [Test]
        public void TryValidate_ShouldNotShowMessageBox_WhenValueIsNotNullOrEmpty()
        {
            mSut.Value = "10000";

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            mSut.TryValidate(messageBoxService);

            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }

        [Test]
        public void TryValidate_ShouldReturnTrue_WhenValueIsNotNullOrEmpty()
        {
            mSut.Value = "10000";

            Assert.IsTrue(mSut.TryValidate(Substitute.For<IMessageBoxService>()));
        }
    }
}
