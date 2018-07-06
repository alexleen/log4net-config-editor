// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Interfaces;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class ForeColorTest
    {
        private ForeColor mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new ForeColor(new ReadOnlyCollection<IProperty>(new List<IProperty>()));
        }

        [Test]
        public void Name_ShouldBeInitializedCorrectly()
        {
            Assert.AreEqual("Foreground:", mSut.Name);
        }

        [Test]
        public void Colors_ShouldBeInitializedProperly()
        {
            Assert.AreEqual(Enum.GetValues(typeof(ConsoleColor)).Cast<ConsoleColor>(), mSut.Colors);
        }

        [Test]
        public void SelectedLevel_ShouldBeNull_ByDefault()
        {
            Assert.IsNull(mSut.SelectedColor);
        }

        [TestCase("<foreColor />", null)]
        [TestCase("<foreColor value=\"\" />", null)]
        [TestCase("<foreColor value=\"DarkBlue\" />", ConsoleColor.DarkBlue)]
        [TestCase("<foreColor value=\"DarkRed\" />", ConsoleColor.DarkRed)]
        [TestCase("<foreColor value=\"whatev\" />", null)]
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

            XmlNode foreColorNode = appender.SelectSingleNode("foreColor");

            Assert.IsNotNull(foreColorNode);
            Assert.AreEqual(ConsoleColor.Blue.ToString(), foreColorNode.Attributes?["value"].Value);
        }
    }
}
