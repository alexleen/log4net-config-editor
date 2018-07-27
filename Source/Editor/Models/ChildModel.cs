// Copyright © 2018 Alex Leendertsen

using Editor.Models.Base;
using System.Xml;

namespace Editor.Models
{
    internal class ChildModel : ModelBase
    {
        public ChildModel(string elementName, XmlNode node)
        {
            ElementName = elementName;
            Node = node;
        }

        public string ElementName { get; }
    }
}
