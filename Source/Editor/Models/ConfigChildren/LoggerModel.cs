// Copyright © 2018 Alex Leendertsen

using System.Xml;
using Editor.Descriptors.Base;
using Editor.Interfaces;
using Editor.Models.Base;

namespace Editor.Models.ConfigChildren
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
