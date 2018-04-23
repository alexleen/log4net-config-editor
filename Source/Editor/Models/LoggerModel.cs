// Copyright © 2018 Alex Leendertsen

using System.Xml;

namespace Editor.Models
{
    public class LoggerModel
    {
        public LoggerModel(string name, XmlNode loggerNode, bool isEnabled)
        {
            Name = name;
            LoggerNode = loggerNode;
            IsEnabled = isEnabled;
        }

        public string Name { get; }

        public XmlNode LoggerNode { get; }

        public bool IsEnabled { get; set; }
    }
}
