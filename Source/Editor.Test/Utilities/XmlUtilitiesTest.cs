// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Editor.Models;
using Editor.Utilities;
using NUnit.Framework;

namespace Editor.Test.Utilities
{
    [TestFixture]
    public class XmlUtilitiesTest
    {
        [Test]
        public void FindAppenderRefs_ShouldReturnAllAppenderRefs()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<log4net>\r\n" +
                           " <appender name=\"appender1\">\r\n" +
                           " </appender>\r\n" +
                           " <appender name=\"appender2\">\r\n" +
                           " </appender>\r\n" +
                           " <appender name=\"appender3\">\r\n" +
                           "  <appender-ref ref=\"appender1\" />\r\n" +
                           " </appender>\r\n" +
                           " <root>\r\n" +
                           "  <appender-ref ref=\"appender1\" />\r\n" +
                           "  <appender-ref ref=\"appender2\" />\r\n" +
                           "  <appender-ref ref=\"appender3\" />\r\n" +
                           " </root>\r\n" +
                           "</log4net>");

            IEnumerable<RefModel> refs = XmlUtilities.FindAppenderRefs(xmlDoc.FirstChild, "appender1");

            Assert.AreEqual(2, refs.Count());
        }

        [Test]
        public void AppendAttribute_ShouldAppendAttribute()
        {
            XmlDocument xmlDoc = new XmlDocument();

            const string name = "name";
            const string value = "value";

            XmlElement xmlElement = xmlDoc.CreateElement("element");
            xmlElement.AppendAttribute(xmlDoc, name, value);

            Assert.AreEqual(value, xmlElement.Attributes[name].Value);
        }

        [Test]
        public void CreateElementWithAttribute_ShouldCreateElementWithAttribute()
        {
            XmlDocument xmlDoc = new XmlDocument();

            const string elementName = "element";
            const string attributeName = "attrName";
            const string attributeValue = "attrValue";

            XmlElement elementWithAttribute = xmlDoc.CreateElementWithAttribute(elementName, attributeName, attributeValue);

            Assert.AreEqual(elementName, elementWithAttribute.Name);
            Assert.AreEqual(attributeValue, elementWithAttribute.Attributes[attributeName].Value);
        }

        [Test]
        public void CreateElementWithValueAttribute_ShouldCreateElementWithValueAttribute()
        {
            XmlDocument xmlDoc = new XmlDocument();

            const string elementName = "element";
            const string attributeValue = "attrValue";

            XmlElement elementWithAttribute = xmlDoc.CreateElementWithValueAttribute(elementName, attributeValue);

            Assert.AreEqual(elementName, elementWithAttribute.Name);
            Assert.AreEqual(attributeValue, elementWithAttribute.Attributes["value"].Value);
        }

        [Test]
        public void CreateElementWithAttributes_ShouldCreateElementWithAttributes()
        {
            (string Name, string Value)[] attributes =
            {
                (Name: "name1", Value: "value1"),
                (Name: "name2", Value: "value2"),
                (Name: "name3", Value: "value3")
            };

            XmlDocument xmlDoc = new XmlDocument();
            const string elementName = "element";
            XmlElement elementWithAttributes = xmlDoc.CreateElementWithAttributes(elementName, attributes);

            Assert.AreEqual(elementName, elementWithAttributes.Name);
            Assert.AreEqual(attributes.Length, elementWithAttributes.Attributes.Count);

            for (int i = 0; i < attributes.Length; i++)
            {
                Assert.AreEqual(attributes[i].Value, elementWithAttributes.Attributes[attributes[i].Name].Value);
            }
        }

        [Test]
        public void AppendTo_ShouldAppendToParent()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode parent = xmlDoc.CreateElement("parent");
            XmlNode child = xmlDoc.CreateElement("child");

            //Test sanity check
            Assert.IsNull(parent.FirstChild);

            child.AppendTo(parent);

            Assert.AreSame(parent.FirstChild, child);
        }

        [Test]
        public void GetValueAttributeValueFromChildElement_ShouldGetValueAttributeValueFromChildElement()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode parent = xmlDoc.CreateElement("parent");
            const string childElementName = "child";
            const string attributeValue = "attrValue";
            XmlNode child = xmlDoc.CreateElementWithValueAttribute(childElementName, attributeValue);
            child.AppendTo(parent);

            Assert.AreEqual(attributeValue, parent.GetValueAttributeValueFromChildElement(childElementName));
        }
    }
}
