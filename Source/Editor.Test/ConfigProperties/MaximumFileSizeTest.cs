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
    public class MaximumFileSizeTest
    {
        private MaximumFileSize mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new MaximumFileSize(new ReadOnlyCollection<IProperty>(new List<IProperty>()));
        }

        [Test]
        public void Value_ShouldBeInitializedToDefault()
        {
            Assert.AreEqual("10MB", mSut.Value);
        }

        [Test]
        public void ToolTip_ShouldBeInitialized()
        {
            Assert.IsNotNull(mSut.ToolTip);
        }

        [TestCase(null, "10MB")]
        [TestCase("<maximumFileSize />", "10MB")]
        [TestCase("<maximumFileSize value=\"\" />", "10MB")]
        [TestCase("<maximumFileSize value=\"100MB\" />", "100MB")]
        public void Load_ShouldLoadCorrectValue(string value, string expected)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender type=\"log4net.Appender.RollingFileAppender\" name=\"rolling\">\n" +
                           $"    {value}\n" +
                           "  </appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(expected, mSut.Value);
        }

        [TestCase("10KB")]
        [TestCase("10MB")]
        [TestCase("10GB")]
        public void TryValidate_ShouldSucceed(string value)
        {
            mSut.Value = value;

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();
            Assert.IsTrue(mSut.TryValidate(messageBoxService));
            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }

        [TestCase("10kb")]
        [TestCase("10")]
        [TestCase("10Gb")]
        public void TryValidate_ShouldFail(string value)
        {
            mSut.Value = value;

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();
            Assert.IsFalse(mSut.TryValidate(messageBoxService));
            messageBoxService.Received(1).ShowError("Maximum file size must end with either \"KB\", \"MB\", or \"GB\".");
        }

        [Test]
        public void Save_ShouldNotSaveIfDefault()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Save(xmlDoc, appender);

            CollectionAssert.IsEmpty(appender.ChildNodes);
        }

        [Test]
        public void Save_ShouldSaveIfNotDefault()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Value = "100MB";
            mSut.Save(xmlDoc, appender);

            XmlNode maxFileSizeNode = appender.SelectSingleNode("maximumFileSize");

            Assert.IsNotNull(maxFileSizeNode);
            Assert.AreEqual(mSut.Value, maxFileSizeNode.Attributes["value"].Value);
        }
    }
}
