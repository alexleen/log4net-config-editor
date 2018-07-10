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
    public class LoggerToMatchTest
    {
        private LoggerToMatch mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new LoggerToMatch(new ReadOnlyCollection<IProperty>(new List<IProperty>()));
        }

        [TestCase("<loggerToMatch />", null)]
        [TestCase("<loggerToMatch value=\"\" />", null)]
        [TestCase("<loggerToMatch value=\"whatev\" />", "whatev")]
        public void Load_ShouldLoadTheCorrectValue(string xml, string expected)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\n" +
                           $"    {xml}\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(expected, mSut.Value);
        }

        [Test]
        public void TryValidate_ShouldSucceed_WhenValueIsNotNullOrEmpty()
        {
            mSut.Value = "whatev";

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();
            Assert.IsTrue(mSut.TryValidate(messageBoxService));
            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }

        [TestCase(null)]
        [TestCase("")]
        public void TryValidate_ShouldFail_WhenValueIsNullOrEmpty(string value)
        {
            mSut.Value = value;

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();
            Assert.IsFalse(mSut.TryValidate(messageBoxService));
            messageBoxService.Received(1).ShowError("'Logger to Match' must be specified.");
        }

        [Test]
        public void Save_ShouldNotSave_WhenValueIsNullOrEmpty()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Save(xmlDoc, appender);

            CollectionAssert.IsEmpty(appender.ChildNodes);
        }

        [Test]
        public void Save_ShouldSave_WhenValueIsNotNullOrEmpty()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Value = "whatev";
            mSut.Save(xmlDoc, appender);

            XmlNode loggerNode = appender.SelectSingleNode("loggerToMatch");

            Assert.IsNotNull(loggerNode);
            Assert.AreEqual(mSut.Value, loggerNode.Attributes?["value"].Value);
        }
    }
}
