// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors;
using Editor.Descriptors.Base;
using System.Xml;

namespace Editor.Models
{
    public class LoggerModel : NamedModel
    {
        public LoggerModel(string name, XmlNode node, bool isEnabled, DescriptorBase descriptor)
            : base(name, node, descriptor)
        {
            IsEnabled = isEnabled;
        }

        public bool IsEnabled { get; set; }
    }
}
