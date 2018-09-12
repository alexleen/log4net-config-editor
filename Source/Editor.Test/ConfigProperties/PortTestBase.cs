// Copyright © 2018 Alex Leendertsen

using System.Net;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public abstract class PortTestBase
    {
        internal Port Sut;

        [SetUp]
        public void SetUp()
        {
            Sut = GetSut();
        }

        internal abstract Port GetSut();

        protected void TestLoadWithXml(string xml, string expected)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\n" +
                           $"      {xml}\n" +
                           "</appender>");

            Sut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(expected, Sut.Value);
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("str", false)]
        [TestCase("-1", false)] //Too small
        [TestCase("65536", false)] //Too big
        [TestCase("1234", true)]
        public void TryValidate_ShouldReturnCorrectValue(string value, bool expectedResult)
        {
            Sut.Value = value;

            Assert.AreEqual(expectedResult, Sut.TryValidate(Substitute.For<IMessageBoxService>()));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("str")]
        [TestCase("-1")] //Too small
        [TestCase("65536")] //Too big
        public void TryValidate_ShouldCallShowError(string value)
        {
            Sut.Value = value;

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();
            Sut.TryValidate(messageBoxService);

            messageBoxService.Received(1).ShowError($"Port must be a valid integer between {IPEndPoint.MinPort} and {IPEndPoint.MaxPort}.");
        }

        [Test]
        public void TryValidate_ShouldNotCallShowError()
        {
            Sut.Value = "1234";

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();
            Sut.TryValidate(messageBoxService);

            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }

        [TestCase(null)]
        [TestCase("")]
        public void Save_ShouldNotSave_WhenNullOrEmptyValue(string value)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appenderElement = xmlDoc.CreateElement("appender");

            Sut.Value = value;
            Sut.Save(xmlDoc, appenderElement);

            CollectionAssert.IsEmpty(appenderElement.ChildNodes);
        }

        protected void TestSave(string elementName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appenderElement = xmlDoc.CreateElement("appender");

            Sut.Value = "1234";
            Sut.Save(xmlDoc, appenderElement);

            XmlNode regexNode = appenderElement.SelectSingleNode(elementName);

            Assert.IsNotNull(regexNode);
            Assert.AreEqual(Sut.Value, regexNode.Attributes["value"].Value);
        }
    }
}
