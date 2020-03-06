// Copyright © 2018 Alex Leendertsen

using System.Xml;
using Editor.ConfigProperties;
using Editor.Utilities;
using log4net.Core;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class LevelPropertyTest
    {
        [SetUp]
        public void SetUp()
        {
            mSut = new LevelProperty();
        }

        private LevelProperty mSut;

        [TestCase("<level />", "ALL")]
        [TestCase("<level value=\"\" />", "ALL")]
        [TestCase("<level value=\"ALL\" />", "ALL")]
        [TestCase("<level value=\"All\" />", "ALL")]
        [TestCase("<level value=\"all\" />", "ALL")]
        public void Load_ShouldLoadTheCorrectValue(string xml, string expected)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\n" +
                           $"    {xml}\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(expected, mSut.SelectedValue);
        }

        [Test]
        public void Ctor_ShouldInitializeNameCorrectly()
        {
            Assert.AreEqual("Level:", mSut.Name);
        }

        [Test]
        public void Levels_ShouldBeAllLevels()
        {
            CollectionAssert.AreEqual(Log4NetUtilities.LevelsByName.Keys, mSut.Values);
        }

        [Test]
        public void Save_ShouldNotSave_WhenUnseleted()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.SelectedValue = null;
            mSut.Save(xmlDoc, appender);

            CollectionAssert.IsEmpty(appender.ChildNodes);
        }

        [Test]
        public void Save_ShouldSaveSelectedLevel()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.SelectedValue = Level.All.Name;
            mSut.Save(xmlDoc, appender);

            XmlNode thresholdNode = appender.SelectSingleNode("level");

            Assert.IsNotNull(thresholdNode);
            Assert.AreEqual(Level.All.Name, thresholdNode.Attributes?["value"].Value);
        }

        [Test]
        public void SelectedLevel_ShouldBeNone()
        {
            Assert.AreEqual(Level.All.Name, mSut.SelectedValue);
        }
    }
}
