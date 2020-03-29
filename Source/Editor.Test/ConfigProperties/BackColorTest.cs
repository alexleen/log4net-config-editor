// Copyright © 2018 Alex Leendertsen

using System;
using System.Linq;
using System.Xml;
using Editor.ConfigProperties;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class BackColorTest
    {
        [SetUp]
        public void SetUp()
        {
            mSut = new BackColor();
        }

        private BackColor mSut;

        [TestCase("<backColor />", null)]
        [TestCase("<backColor value=\"\" />", null)]
        [TestCase("<backColor value=\"DarkBlue\" />", ConsoleColor.DarkBlue)]
        [TestCase("<backColor value=\"DarkRed\" />", ConsoleColor.DarkRed)]
        [TestCase("<backColor value=\"whatev\" />", null)]
        public void Load_ShouldLoadTheCorrectValue(string xml, ConsoleColor? expected)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\n" +
                           $"    {xml}\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(expected, mSut.SelectedColor);
        }

        [Test]
        public void Colors_ShouldBeInitializedProperly()
        {
            Assert.AreEqual(Enum.GetValues(typeof(ConsoleColor)).Cast<ConsoleColor>(), mSut.Colors);
        }

        [Test]
        public void Name_ShouldBeInitializedCorrectly()
        {
            Assert.AreEqual("Background:", mSut.Name);
        }

        [Test]
        public void Save_ShouldNotSave_WhenUnseleted()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("mapping");

            mSut.Save(xmlDoc, appender);

            CollectionAssert.IsEmpty(appender.ChildNodes);
        }

        [Test]
        public void Save_ShouldSaveSelectedLevel()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("mapping");

            mSut.SelectedColor = ConsoleColor.Blue;
            mSut.Save(xmlDoc, appender);

            XmlNode backColorNode = appender.SelectSingleNode("backColor");

            Assert.IsNotNull(backColorNode);
            Assert.AreEqual(ConsoleColor.Blue.ToString(), backColorNode.Attributes?["value"].Value);
        }

        [Test]
        public void SelectedLevel_ShouldBeNull_ByDefault()
        {
            Assert.IsNull(mSut.SelectedColor);
        }
    }
}
