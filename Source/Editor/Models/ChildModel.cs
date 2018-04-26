// Copyright © 2018 Alex Leendertsen

using System.Xml;

namespace Editor.Models
{
    public class ChildModel
    {
        public ChildModel(string elementName, XmlNode node)
        {
            ElementName = elementName;
            Node = node;
        }

        public string ElementName { get; }

        public XmlNode Node { get; }
    }
}
