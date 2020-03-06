// Copyright © 2020 Alex Leendertsen

using System.Xml;
using Editor.ConfigProperties;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class EncodingTest
    {
        [SetUp]
        public void SetUp()
        {
            mSut = new Encoding("Encoding:", "encoding");
        }

        private Encoding mSut;

        [TestCase("", "")]
        [TestCase("us-ascii", "us-ascii")]
        [TestCase("utf-16", "utf-16")]
        [TestCase("utf-16BE", "utf-16BE")]
        [TestCase("utf-7", "utf-7")]
        [TestCase("utf-8", "utf-8")]
        [TestCase("utf-32", "utf-32")]
        [TestCase("unknown", "")]
        public void Load_ShouldLoadCorrectValue(string value, string expected)
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlAttribute valueAttribute = xmlDoc.CreateAttribute("value");
            valueAttribute.Value = value;

            XmlElement encodingElement = xmlDoc.CreateElement("encoding");
            encodingElement.Attributes.Append(valueAttribute);

            XmlElement appenderElement = xmlDoc.CreateElement("appender");
            appenderElement.AppendChild(encodingElement);

            mSut.Load(appenderElement);

            Assert.AreEqual(expected, mSut.SelectedValue);
        }

        [Test]
        public void Ctor_ShouldInitializeSelectedValueToEmpty()
        {
            Assert.AreEqual(string.Empty, mSut.SelectedValue);
        }

        [Test]
        public void Save_ShouldNotSaveSelectedValue_WhenNotSelected()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appenderElement = xmlDoc.CreateElement("appender");

            mSut.Save(xmlDoc, appenderElement);

            Assert.IsNull(appenderElement.SelectSingleNode("encoding"));
        }

        [Test]
        public void Save_ShouldSaveSelectedValue_WhenSelected()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appenderElement = xmlDoc.CreateElement("appender");

            mSut.SelectedValue = "whatev";
            mSut.Save(xmlDoc, appenderElement);

            XmlNode encodingElement = appenderElement.SelectSingleNode("encoding");
            Assert.IsNotNull(encodingElement);
            Assert.AreEqual(mSut.SelectedValue, encodingElement.Attributes["value"].Value);
        }
    }
}
