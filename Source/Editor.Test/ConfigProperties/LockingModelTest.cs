// Copyright © 2020 Alex Leendertsen

using System.Xml;
using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.XML;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class LockingModelTest
    {
        [SetUp]
        public void SetUp()
        {
            mSut = new LockingModel();
        }

        private LockingModel mSut;

        [TestCase(null, "Exclusive")]
        [TestCase("<lockingModel />", "Exclusive")]
        [TestCase("<lockingModel type=\"\" />", "Exclusive")]
        [TestCase("<lockingModel type=\"log4net.Appender.FileAppender+Whatev\" />", "Exclusive")]
        [TestCase("<lockingModel type=\"log4net.Appender.FileAppender+MinimalLock\" />", "Minimal")]
        public void Load_ShouldLoadCorrectly(string xml, string expected)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\n" +
                           $"      {xml}\n" +
                           "</appender>");

            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.OriginalNode.Returns(xmlDoc.FirstChild);

            mSut.Load(config);

            Assert.AreEqual(expected, mSut.SelectedModel.Name);
        }

        [Test]
        public void LockingModels_ShouldBeInitializedCorrectly()
        {
            CollectionAssert.AreEqual(new[] { LockingModelDescriptor.Exclusive, LockingModelDescriptor.Minimal, LockingModelDescriptor.InterProcess }, mSut.LockingModels);
        }

        [Test]
        public void Save_ShouldNotSaveIfExclusive()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.Save(config);

            config.DidNotReceiveWithAnyArgs().Save();
        }

        [Test]
        public void Save_ShouldSaveIfNotExclusive()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.SelectedModel = LockingModelDescriptor.Minimal;

            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            mSut.Save(config);

            XmlNode modelNode = appender.SelectSingleNode("lockingModel");

            config.Received(1).Save(new Element("lockingModel", new[] { ("type", "log4net.Appender.FileAppender+MinimalLock") }));
        }

        [Test]
        public void SelectedModel_ShouldBeInitializedToExclusive()
        {
            Assert.AreEqual(LockingModelDescriptor.Exclusive, mSut.SelectedModel);
        }
    }
}
