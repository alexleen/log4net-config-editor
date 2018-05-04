// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Xml;
using Editor.Models;

namespace Editor.Utilities
{
    public static class XmlUtilities
    {
        /// <summary>
        /// Finds all instances of the 'appender-ref' element with the specified appender name within the specified log4net element.
        /// </summary>
        /// <param name="log4NetNode"></param>
        /// <param name="appenderName"></param>
        /// <returns></returns>
        public static IEnumerable<RefModel> FindAppenderRefs(XmlNode log4NetNode, string appenderName)
        {
            XmlNodeList appenderRefs = log4NetNode.SelectNodes("//appender-ref");

            foreach (XmlNode appenderRef in appenderRefs)
            {
                if (appenderRef.Attributes?["ref"].Value == appenderName)
                {
                    yield return new RefModel(appenderRef);
                }
            }
        }

        /// <summary>
        /// Adds the specified child node to the specified parent node if the child node does not already exist.
        /// If the child node already exists, it's attributes are replaced by the specified child's.
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="childNode"></param>
        public static void AddOrUpdate(XmlNode parentNode, XmlNode childNode)
        {
            XmlNode childMatch = parentNode.SelectSingleNode(childNode.Name);

            if (childMatch == null)
            {
                parentNode.AppendChild(childNode);
            }
            else
            {
                parentNode.ReplaceChild(childNode, childMatch);
            }
        }

        public static void AppendAttribute(this XmlNode element, XmlDocument xmlDoc, string name, string value)
        {
            XmlAttribute attr = xmlDoc.CreateAttribute(name);
            attr.Value = value;
            element.Attributes.Append(attr);
        }

        public static XmlElement CreateElementWithAttribute(this XmlDocument xmlDoc, string elementName, string attributeName, string attributeValue)
        {
            XmlElement element = xmlDoc.CreateElement(elementName);
            element.AppendAttribute(xmlDoc, attributeName, attributeValue);
            return element;
        }

        public static XmlElement CreateElementWithAttributes(this XmlDocument xmlDoc, string elementName, IEnumerable<(string Name, string Value)> attributes)
        {
            XmlElement element = xmlDoc.CreateElement(elementName);

            foreach ((string name, string value) in attributes)
            {
                element.AppendAttribute(xmlDoc, name, value);
            }

            return element;
        }

        /// <summary>
        /// Appends this node to the specified parent.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="parent"></param>
        public static void AppendTo(this XmlNode node, XmlNode parent)
        {
            parent.AppendChild(node);
        }

        /// <summary>
        /// Retrieves the value for an attribute with name 'value' from a child of this node.
        /// Returns null if not found.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="childElementName"></param>
        /// <returns></returns>
        public static string GetValueAttributeValueFromChildElement(this XmlNode node, string childElementName)
        {
            return node[childElementName]?.Attributes["value"]?.Value;
        }
    }
}
