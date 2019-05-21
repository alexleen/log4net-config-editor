// Copyright © 2018 Alex Leendertsen

using System.Xml;
using Editor.Descriptors;
using Editor.Models.Base;

namespace Editor.Models.ConfigChildren
{
    public class AppenderModel : NamedModel
    {
        public AppenderModel(AppenderDescriptor descriptor, XmlNode node, int incomingReferences)
            : base(node, descriptor)
        {
            IncomingReferences = incomingReferences;
        }

        public int IncomingReferences { get; }

        public bool IsEnabled { get; set; }
    }
}
