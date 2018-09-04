// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors;
using Editor.Descriptors.Base;
using Editor.Interfaces;
using System.Xml;

namespace Editor.Models
{
    public class LoggerModel : NamedModel, IAcceptAppenderRef
    {
        public LoggerModel(XmlNode node, bool isEnabled, DescriptorBase descriptor)
            : base(node, descriptor)
        {
            IsEnabled = isEnabled;
        }

        public bool IsEnabled { get; set; }
    }
}
