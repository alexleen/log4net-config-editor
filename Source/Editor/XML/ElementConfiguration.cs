// Copyright © 2020 Alex Leendertsen

using System;
using System.Linq;
using System.Xml;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.XML
{
    internal class ElementConfiguration : IElementConfiguration
    {
        public ElementConfiguration(XmlDocument xmlDocument, XmlNode log4NetNode, XmlNode originalNode, XmlNode newNode)
        {
            ConfigXml = xmlDocument;
            Log4NetNode = log4NetNode;
            OriginalNode = originalNode;
            NewNode = newNode;
        }

        public ElementConfiguration(IConfiguration configuration, XmlNode originalNode, XmlNode newNode)
            : this(configuration.ConfigXml, configuration.Log4NetNode, originalNode, newNode)
        {
        }

        public XmlNode OriginalNode { get; }

        public bool TryGetAttributeValueOfChildElement(string elementName, string attributeName, out IValueResult result)
        {
            //Find the element
            XmlNode element = OriginalNode.ChildNodes.Cast<XmlNode>().FirstOrDefault(n => string.Equals(n.LocalName, elementName, StringComparison.OrdinalIgnoreCase));

            //Find the attribute on the element
            XmlAttribute attr = element?.Attributes?.Cast<XmlAttribute>().FirstOrDefault(a => string.Equals(a.LocalName, attributeName, StringComparison.OrdinalIgnoreCase));

            if (attr == null)
            {
                result = null;
                return false;
            }

            result = new ValueResult(element.LocalName, attr.LocalName, attr.Value);
            return true;
        }

        public XmlNode NewNode { get; }

        public void SaveAs(string elementName, string attributeName, string value)
        {
            ConfigXml.CreateElementWithAttribute(elementName, attributeName, value).AppendTo(NewNode);
        }

        public XmlDocument ConfigXml { get; }

        public XmlNode Log4NetNode { get; }

        private class ValueResult : IValueResult
        {
            public ValueResult(string actualElementName, string actualAttributeName, string attributeValue)
            {
                ActualElementName = actualElementName;
                ActualAttributeName = actualAttributeName;
                AttributeValue = attributeValue;
            }

            public string ActualElementName { get; }

            public string ActualAttributeName { get; }

            public string AttributeValue { get; }
        }
    }
}
