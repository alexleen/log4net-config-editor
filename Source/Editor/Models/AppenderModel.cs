// Copyright © 2018 Alex Leendertsen

using System.Xml;
using Editor.Descriptors;

namespace Editor.Models
{
    internal class AppenderModel : ChildModel
    {
        public AppenderModel(AppenderDescriptor descriptor, XmlNode appender)
            : base("appender", appender)
        {
            Descriptor = descriptor;
            Name = appender.Attributes?["name"].Value;
        }

        public AppenderDescriptor Descriptor { get; }

        public string Name { get; }
    }
}
