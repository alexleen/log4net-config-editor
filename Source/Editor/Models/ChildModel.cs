// Copyright © 2018 Alex Leendertsen

using System.Xml;
using Editor.Models.Base;

namespace Editor.Models
{
    public class ChildModel : ModelBase
    {
        public ChildModel(string elementName, XmlNode node)
        {
            ElementName = elementName;
            Node = node;
        }

        public string ElementName { get; }
    }
}
