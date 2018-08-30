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
        public static IEnumerable<XmlNode> FindAppenderRefs(XmlNode log4NetNode, string appenderName)
        {
            XmlNodeList appenderRefs = log4NetNode.SelectNodes("//appender-ref");

            foreach (XmlNode appenderRef in appenderRefs)
            {
                if (appenderRef.Attributes?["ref"].Value == appenderName)
                {
                    yield return appenderRef;
                }
            }
        }

        internal static IEnumerable<LoggerModel> FindAvailableAppenderRefLocations(XmlNode log4NetNode)
        {
            List<LoggerModel> loggers = new List<LoggerModel>();

            loggers.AddRange(CreateLoggerModelsFromNodes(log4NetNode, "appender[@type='Log4Net.Async.AsyncForwardingAppender,Log4Net.Async']"));
            loggers.AddRange(GetRootLoggerAndLoggers(log4NetNode));

            return loggers;
        }

        internal static IEnumerable<LoggerModel> GetRootLoggerAndLoggers(XmlNode log4NetNode)
        {
            List<LoggerModel> loggers = new List<LoggerModel>();

            loggers.AddRange(CreateLoggerModelsFromNodes(log4NetNode, "logger"));

            XmlNode root = log4NetNode.SelectSingleNode("root");

            if (root != null)
            {
                loggers.Add(new LoggerModel("root", "root", root, false));
            }

            return loggers;
        }

        private static IEnumerable<LoggerModel> CreateLoggerModelsFromNodes(XmlNode log4NetNode, string nodesXPath)
        {
            XmlNodeList nodes = log4NetNode.SelectNodes(nodesXPath);

            foreach (XmlNode node in nodes)
            {
                string name = node.Attributes["name"]?.Value;

                if (!string.IsNullOrEmpty(name))
                {
                    yield return new LoggerModel(node.Name, name, node, false);
                }
            }
        }

        /// <summary>
        /// Adds the specified <paramref name="appenderName"/> to the specified <paramref name="node"/> as an appender-ref child element.
        /// If an appender-ref for this <paramref name="appenderName"/> already exists, this method does nothing.
        /// If more than one appender-ref for this <paramref name="appenderName"/> exists, the count is reduced to one.
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="node"></param>
        /// <param name="appenderName"></param>
        public static void AddAppenderRefToNode(XmlDocument xmlDoc, XmlNode node, string appenderName)
        {
            XmlNodeList appenderRefs = node.SelectNodes($"appender-ref[@ref='{appenderName}']");

            if (appenderRefs != null)
            {
                if (appenderRefs.Count == 1)
                {
                    //Only one - we're good
                    return;
                }

                if (appenderRefs.Count >= 1)
                {
                    //More than one - remove all
                    foreach (XmlNode appenderRef in appenderRefs)
                    {
                        node.RemoveChild(appenderRef);
                    }
                }
            }

            //Add
            xmlDoc.CreateElementWithAttribute("appender-ref", "ref", appenderName).AppendTo(node);
        }

        /// <summary>
        /// Adds an attribute to this element with the specified name and value.
        /// If an attribute with the same name already exists, it is replaced by an attribute with the specified value.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="xmlDoc"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
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

        public static XmlElement CreateElementWithValueAttribute(this XmlDocument xmlDoc, string elementName, string attributeValue)
        {
            return xmlDoc.CreateElementWithAttribute(elementName, "value", attributeValue);
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
