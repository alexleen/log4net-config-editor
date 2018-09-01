// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors;
using System.Xml;

namespace Editor.Models
{
    public class AppenderModel : NamedModel
    {
        public AppenderModel(AppenderDescriptor descriptor, XmlNode node, string name, int incomingReferences)
            : base(name, node, descriptor)
        {
            IncomingReferences = incomingReferences;
        }

        public int IncomingReferences { get; }

        public bool IsEnabled { get; set; }
    }
}
