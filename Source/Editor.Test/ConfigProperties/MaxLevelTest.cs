// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Interfaces;
using Editor.Utilities;
using log4net.Core;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class MaxLevelTest
    {
        private MaxLevel mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new MaxLevel(new ReadOnlyCollection<IProperty>(new List<IProperty>()));
        }

        [Test]
        public void Ctor_ShouldInitalizeNameCorrectly()
        {
            Assert.AreEqual("Max Level:", mSut.Name);
        }

        [Test]
        public void Levels_ShouldBeAllLevels()
        {
            CollectionAssert.AreEqual(new[] { string.Empty }.Concat(Log4NetUtilities.LevelsByName.Keys), mSut.Levels);
        }

        [Test]
        public void SelectedLevel_ShouldBeEmpty()
        {
            Assert.AreEqual(string.Empty, mSut.SelectedLevel);
        }

        [TestCase("<levelMax />", "")]
        [TestCase("<levelMax value=\"\" />", "")]
        [TestCase("<levelMax value=\"ALL\" />", "ALL")]
        [TestCase("<levelMax value=\"All\" />", "ALL")]
        [TestCase("<levelMax value=\"all\" />", "ALL")]
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

            XmlNode levelNode = appender.SelectSingleNode("levelMax");

            Assert.IsNotNull(levelNode);
            Assert.AreEqual(Level.All.Name, levelNode.Attributes?["value"].Value);
        }

        [Test]
        public void Save_ShouldNotSaveSelectedLevel_WhenNotSelected()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Save(xmlDoc, appender);

            CollectionAssert.IsEmpty(appender.ChildNodes);
        }
    }
}
