// Copyright © 2018 Alex Leendertsen

using System.Xml;
using Editor.Interfaces;

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

        public XmlNode NewNode { get; }

        public XmlDocument ConfigXml { get; }

        public XmlNode Log4NetNode { get; }
    }
}
