// Copyright © 2020 Alex Leendertsen

using System;
using System.Linq;
using System.Net.Mail;
using System.Xml;
using Editor.ConfigProperties.Base;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties.Base
{
    [TestFixture]
    public class EnumPropertyTest
    {
        [SetUp]
        public void SetUp()
        {
            mSut = new EnumProperty<MailPriority>("name", 100, "elementName");
        }

        private EnumProperty<MailPriority> mSut;

        [Test]
        public void Ctor_ShouldSelectFirstValue()
        {
            Assert.AreEqual(mSut.Values.First(), mSut.SelectedValue);
        }

        [Test]
        public void Ctor_ShouldUseStringValues()
        {
            CollectionAssert.AreEquivalent(Enum.GetNames(typeof(MailPriority)), mSut.Values);
        }

        [Test]
        public void Load_ShouldLoadValue()
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlAttribute valueAttribute = xmlDoc.CreateAttribute("value");
            valueAttribute.Value = MailPriority.High.ToString();

            XmlElement encodingElement = xmlDoc.CreateElement("elementName");
            encodingElement.Attributes.Append(valueAttribute);

            XmlElement appenderElement = xmlDoc.CreateElement("appender");
            appenderElement.AppendChild(encodingElement);

            mSut.Load(appenderElement);

            Assert.AreEqual(MailPriority.High.ToString(), mSut.SelectedValue);
        }

        [Test]
        public void Load_ShouldNotLoadValuesItCannotParse()
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlAttribute valueAttribute = xmlDoc.CreateAttribute("value");
            valueAttribute.Value = "whatev"; // Not a valid MailPriority enum value

            XmlElement encodingElement = xmlDoc.CreateElement("elementName");
            encodingElement.Attributes.Append(valueAttribute);

            XmlElement appenderElement = xmlDoc.CreateElement("appender");
            appenderElement.AppendChild(encodingElement);

            mSut.Load(appenderElement);

            Assert.AreEqual(mSut.Values.First(), mSut.SelectedValue);
        }

        [Test]
        public void Save_ShouldSave()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appenderElement = xmlDoc.CreateElement("appender");

            mSut.SelectedValue = MailPriority.High.ToString();
            mSut.Save(xmlDoc, appenderElement);

            XmlNode element = appenderElement.SelectSingleNode("elementName");
            Assert.IsNotNull(element);
            Assert.AreEqual(mSut.SelectedValue, element.Attributes["value"].Value);
        }
    }
}
