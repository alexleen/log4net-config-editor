// Copyright © 2020 Alex Leendertsen

using System.Net;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class PortTest
    {
        [SetUp]
        public void SetUp()
        {
            mSut = new Port("Port:", "port", null);
        }

        private Port mSut;

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("str", false)]
        [TestCase("-1", false)] //Too small
        [TestCase("65536", false)] //Too big
        [TestCase("1234", true)]
        public void TryValidate_ShouldReturnCorrectValue(string value, bool expectedResult)
        {
            mSut.Value = value;

            Assert.AreEqual(expectedResult, mSut.TryValidate(Substitute.For<IMessageBoxService>()));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("str")]
        [TestCase("-1")] //Too small
        [TestCase("65536")] //Too big
        public void TryValidate_ShouldCallShowError(string value)
        {
            mSut.Value = value;

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();
            mSut.TryValidate(messageBoxService);

            messageBoxService.Received(1).ShowError($"Port must be a valid integer between {IPEndPoint.MinPort} and {IPEndPoint.MaxPort}.");
        }

        [TestCase(null)]
        [TestCase("")]
        public void Save_ShouldNotSave_WhenNullOrEmptyValue(string value)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appenderElement = xmlDoc.CreateElement("appender");

            mSut.Value = value;
            mSut.Save(xmlDoc, appenderElement);

            CollectionAssert.IsEmpty(appenderElement.ChildNodes);
        }

        [Test]
        public void Save_ShouldNotSave_WhenDefaultValue()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appenderElement = xmlDoc.CreateElement("appender");

            mSut = new Port("Port:", "port", 123);
            mSut.Value = "123";
            mSut.Save(xmlDoc, appenderElement);

            CollectionAssert.IsEmpty(appenderElement.ChildNodes);
        }

        [Test]
        public void Save_ShouldSave()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appenderElement = xmlDoc.CreateElement("appender");

            mSut.Value = "1234";
            mSut.Save(xmlDoc, appenderElement);

            XmlNode portNode = appenderElement.SelectSingleNode("port");

            Assert.IsNotNull(portNode);
            Assert.AreEqual(mSut.Value, portNode.Attributes["value"].Value);
        }

        [Test]
        public void TryValidate_ShouldNotCallShowError()
        {
            mSut.Value = "1234";

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();
            mSut.TryValidate(messageBoxService);

            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }
    }
}
