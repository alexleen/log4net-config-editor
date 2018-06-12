// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Xml;
using Editor.Descriptors;
using Editor.Windows.Appenders.Properties;
using Editor.Windows.PropertyCommon;
using NUnit.Framework;

namespace Editor.Test.Windows.Appenders.Properties
{
    [TestFixture]
    public class LockingModelTest
    {
        private LockingModel mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new LockingModel(new ObservableCollection<IProperty>());
        }

        [Test]
        public void LockingModels_ShouldBeInitializedCorrectly()
        {
            CollectionAssert.AreEqual(new[] { LockingModelDescriptor.Exclusive, LockingModelDescriptor.Minimal, LockingModelDescriptor.InterProcess }, mSut.LockingModels);
        }

        [Test]
        public void SelectedModel_ShouldBeInitializedToExclusive()
        {
            Assert.AreEqual(LockingModelDescriptor.Exclusive, mSut.SelectedModel);
        }

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

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(expected, mSut.SelectedModel.Name);
        }

        [Test]
        public void Save_ShouldNotSaveIfExclusive()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Save(xmlDoc, appender);

            CollectionAssert.IsEmpty(appender.ChildNodes);
        }

        [Test]
        public void Save_ShouldSaveIfNotExclusive()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.SelectedModel = LockingModelDescriptor.Minimal;
            mSut.Save(xmlDoc, appender);

            XmlNode modelNode = appender.SelectSingleNode("lockingModel");

            Assert.IsNotNull(modelNode);
            Assert.AreEqual(LockingModelDescriptor.Minimal.TypeNamespace, modelNode.Attributes?["type"].Value);
        }
    }
}
