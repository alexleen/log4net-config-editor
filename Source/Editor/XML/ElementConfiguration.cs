// Copyright © 2020 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.XML
{
    //TODO two implementations: one for a new element (no original node) and one for an existing one (original and new nodes)
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

        public bool Load(string attributeName, out IValueResult result, params string[] childElementNames)
        {
            XmlNode child = OriginalNode;
            IList<string> actualNames = new List<string>();

            if (childElementNames.Length > 0)
            {
                child = OriginalNode.ChildNodes.Cast<XmlNode>().FirstOrDefault(n => string.Equals(n.LocalName, childElementNames.First(), StringComparison.OrdinalIgnoreCase));

                if (child != null)
                {
                    actualNames.Add(child.LocalName);
                }

                foreach (string childElementName in childElementNames.Skip(1))
                {
                    child = child?.ChildNodes.Cast<XmlNode>().FirstOrDefault(n => string.Equals(n.LocalName, childElementName, StringComparison.OrdinalIgnoreCase));

                    if (child == null)
                    {
                        break;
                    }

                    actualNames.Add(child.LocalName);
                }
            }

            //Find the attribute on the element
            XmlAttribute attr = child?.Attributes?.Cast<XmlAttribute>().FirstOrDefault(a => string.Equals(a.LocalName, attributeName, StringComparison.OrdinalIgnoreCase));

            if (attr == null)
            {
                result = null;
                return false;
            }

            result = new ValueResult(actualNames, attr.LocalName, attr.Value);
            return true;
        }

        public IEnumerable<XmlNode> FindOriginalNodeChildren(string childName)
        {
            return XmlUtilities.FindNodeChildrenCaseInsensitive(OriginalNode, childName);
        }

        public IEnumerable<XmlNode> FindLog4NetNodeChildren(string childName)
        {
            return XmlUtilities.FindNodeChildrenCaseInsensitive(Log4NetNode, childName);
        }

        public XmlNode NewNode { get; }

        public void Save(params IElement[] children)
        {
            SaveTo(NewNode, children);
        }

        public void SaveToNode(XmlNode parent, params IElement[] children)
        {
            SaveTo(parent, children);
        }

        private void SaveTo(XmlNode node, params IElement[] children)
        {
            foreach (IElement element in children)
            {
                XmlNode child = ConfigXml.CreateElement(element.Name);
                foreach ((string attrName, string attrValue) in element.Attributes)
                {
                    child.AppendAttribute(ConfigXml, attrName, attrValue);
                }

                node.AppendChild(child);
            }
        }

        public void SaveHierarchical(params IElement[] children)
        {
            IElement element = children.First();
            XmlNode firstChild = ConfigXml.CreateElement(element.Name);
            foreach ((string attrName, string attrValue) in element.Attributes)
            {
                firstChild.AppendAttribute(ConfigXml, attrName, attrValue);
            }

            XmlNode lastChild = firstChild;
            foreach (IElement e in children.Skip(1))
            {
                XmlElement temp = ConfigXml.CreateElement(e.Name);
                foreach ((string attrName, string attrValue) in e.Attributes)
                {
                    temp.AppendAttribute(ConfigXml, attrName, attrValue);
                }

                lastChild.AppendChild(temp);
                lastChild = temp;
            }

            NewNode.AppendChild(firstChild);
        }

        public void Save(string attributeName, string value)
        {
            NewNode.AppendAttribute(ConfigXml, attributeName, value);
        }

        public XmlDocument ConfigXml { get; }

        public XmlNode Log4NetNode { get; }

        private class ValueResult : IValueResult
        {
            public ValueResult(IEnumerable<string> actualElementNames, string actualAttributeName, string attributeValue)
            {
                ActualElementNames = actualElementNames;
                ActualAttributeName = actualAttributeName;
                AttributeValue = attributeValue;
            }

            public IEnumerable<string> ActualElementNames { get; }

            public string ActualAttributeName { get; }

            public string AttributeValue { get; }
        }
    }
}
