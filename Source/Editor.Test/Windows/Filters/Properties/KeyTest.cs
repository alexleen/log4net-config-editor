// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Xml;
using Editor.Windows;
using Editor.Windows.Filters.Properties;
using Editor.Windows.PropertyCommon;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.Windows.Filters.Properties
{
    [TestFixture]
    public class KeyTest
    {
        private Key mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new Key(new ObservableCollection<IProperty>());
        }

        [TestCase("<key />", null)]
        [TestCase("<key value=\"\" />", null)]
        [TestCase("<key value=\"whatev\" />", "whatev")]
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
            messageBoxService.Received(1).ShowError("'Key' must be specified.");
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

            XmlNode keyNode = appender.SelectSingleNode("key");

            Assert.IsNotNull(keyNode);
            Assert.AreEqual(mSut.Value, keyNode.Attributes?["value"].Value);
        }
    }
}
