// Copyright © 2018 Alex Leendertsen

using System.Xml;

namespace Editor.Models
{
    public class LoggerModel : ChildModel
    {
        public LoggerModel(string elementName, string name, XmlNode node, bool isEnabled)
            : base(elementName, node)
        {
            Name = name;
            IsEnabled = isEnabled;
        }

        public string Name { get; }

        public bool IsEnabled { get; set; }
    }
}
