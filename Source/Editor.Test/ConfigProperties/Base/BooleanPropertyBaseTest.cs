// Copyright © 2020 Alex Leendertsen

using System;
using System.Xml;
using Editor.ConfigProperties.Base;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties.Base
{
    [TestFixture]
    public class BooleanPropertyBaseTest
    {
        [SetUp]
        public void SetUp()
        {
            mDefaultValue = true;
            mName = "name";
            mSut = new BooleanPropertyBase(mName, "elementName", mDefaultValue);
        }

        private string mName;
        private bool mDefaultValue;
        private BooleanPropertyBase mSut;

        [TestCase("<boolean />", true)]
        [TestCase("<boolean><elementName/></boolean>", true)]
        [TestCase("<boolean><elementName value=\"\"/></boolean>", true)]
        [TestCase("<boolean><elementName value=\"whatev\"/></boolean>", true)]
        [TestCase("<boolean><elementName value=\"False\"/></boolean>", false)]
        [TestCase("<boolean><elementName value=\"false\"/></boolean>", false)]
        public void Load_ShouldLoadTheCorrectValue(string xml, bool expected)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(expected, mSut.Value);
        }

        [Test]
        public void Ctor_ShouldAssignName()
        {
            Assert.AreEqual(mName, mSut.Name);
        }

        [Test]
        public void Ctor_ShouldAssignValueToDefault()
        {
            Assert.AreEqual(mDefaultValue, mSut.Value);
        }

        [Test]
        public void Ctor_ShouldThrowIfElementNameIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BooleanPropertyBase("name", null, true));
        }

        [Test]
        public void Ctor_ShouldThrowIfNameIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BooleanPropertyBase(null, "elementName", true));
        }

        [Test]
        public void Save_ShouldIgnoreDefaultValue()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement node = xmlDoc.CreateElement("boolean");

            mSut.Save(xmlDoc, node);

            CollectionAssert.IsEmpty(node.ChildNodes);
        }

        [Test]
        public void Save_ShouldSaveNonDefaultValue()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement node = xmlDoc.CreateElement("boolean");

            mSut.Value = !mDefaultValue;
            mSut.Save(xmlDoc, node);

            CollectionAssert.IsNotEmpty(node.ChildNodes);
        }
    }
}
