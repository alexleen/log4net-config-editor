// Copyright © 2018 Alex Leendertsen

using System.Xml;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Models.ConfigChildren
{
    internal class AsyncAppenderModel : AppenderModel, IAcceptAppenderRef
    {
        public AsyncAppenderModel(XmlNode node, int incomingReferences)
            : base(AppenderDescriptor.Async, node, incomingReferences)
        {
        }
    }
}
