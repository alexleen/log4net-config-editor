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
    public class MaxSizeRollBackupsTest
    {
        private MaxSizeRollBackups mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new MaxSizeRollBackups(new ReadOnlyCollection<IProperty>(new List<IProperty>()));
        }

        [Test]
        public void ToolTip_ShouldBeInitialized()
        {
            Assert.IsNotNull(mSut.ToolTip);
        }

        [TestCase(null, null)]
        [TestCase("<maxSizeRollBackups />", null)]
        [TestCase("<maxSizeRollBackups value=\"\" />", null)]
        [TestCase("<maxSizeRollBackups value=\"str\" />", "str")]
        [TestCase("<maxSizeRollBackups value=\"10\" />", "10")]
        public void Load_ShouldLoadCorrectly(string xml, string expected)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender name=\"asyncAppender\" type=\"Log4Net.Async.AsyncForwardingAppender,Log4Net.Async\">\n" +
                           $"      {xml}\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(expected, mSut.Value);
        }

        [Test]
        public void TryValidate_ShouldSucceed_WhenInt()
        {
            mSut.Value = "1";

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();
            Assert.IsTrue(mSut.TryValidate(messageBoxService));
            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }

        [Test]
        public void TryValidate_ShouldNotSucceed_WhenNotInt()
        {
            mSut.Value = "str";

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();
            Assert.IsFalse(mSut.TryValidate(messageBoxService));
            messageBoxService.Received(1).ShowError("Max size roll backups must be a valid integer.");
        }

        [Test]
        public void Save_ShouldSaveCorrectly()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Value = "1";
            mSut.Save(xmlDoc, appender);

            XmlNode maxSizeNode = appender.SelectSingleNode("maxSizeRollBackups");

            Assert.IsNotNull(maxSizeNode);
            Assert.AreEqual("1", maxSizeNode.Attributes?["value"].Value);
        }
    }
}
