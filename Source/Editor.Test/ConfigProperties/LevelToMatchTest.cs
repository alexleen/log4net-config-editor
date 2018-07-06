// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Interfaces;
using Editor.Utilities;
using log4net.Core;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class LevelToMatchTest
    {
        private LevelToMatch mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new LevelToMatch(new ReadOnlyCollection<IProperty>(new List<IProperty>()));
        }

        [Test]
        public void Ctor_ShouldInitializeNameCorrectly()
        {
            Assert.AreEqual("Level to Match:", mSut.Name);
        }

        [Test]
        public void Levels_ShouldBeAllLevels()
        {
            CollectionAssert.AreEqual(Log4NetUtilities.LevelsByName.Keys, mSut.Levels);
        }

        [Test]
        public void SelectedLevel_ShouldBeAll()
        {
            Assert.AreEqual(Level.All.Name, mSut.SelectedLevel);
        }

        [TestCase("<levelToMatch />", "ALL")]
        [TestCase("<levelToMatch value=\"\" />", "ALL")]
        [TestCase("<levelToMatch value=\"ALL\" />", "ALL")]
        [TestCase("<levelToMatch value=\"All\" />", "ALL")]
        [TestCase("<levelToMatch value=\"all\" />", "ALL")]
        public void Load_ShouldLoadTheCorrectValue(string xml, string expected)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\n" +
                           $"    {xml}\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(expected, mSut.SelectedLevel);
        }

        [Test]
        public void Save_ShouldSaveSelectedLevel()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.SelectedLevel = Level.All.Name;
            mSut.Save(xmlDoc, appender);

            XmlNode thresholdNode = appender.SelectSingleNode("levelToMatch");

            Assert.IsNotNull(thresholdNode);
            Assert.AreEqual(Level.All.Name, thresholdNode.Attributes?["value"].Value);
        }
    }
}
