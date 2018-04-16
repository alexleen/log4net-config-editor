// Copyright © 2018 Alex Leendertsen

using System.Xml;
using Editor.Descriptors;

namespace Editor.Models
{
    internal class AppenderModel
    {
        public AppenderModel(AppenderDescriptor descriptor, XmlNode appender)
        {
            Descriptor = descriptor;
            Node = appender;
            Name = appender.Attributes?["name"].Value;
        }

        public AppenderDescriptor Descriptor { get; }

        public XmlNode Node { get; }

        public string Name { get; }
    }
}
