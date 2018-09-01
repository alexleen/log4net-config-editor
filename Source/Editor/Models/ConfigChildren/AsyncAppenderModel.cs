using Editor.Descriptors;
using Editor.Interfaces;
using System.Xml;

namespace Editor.Models.ConfigChildren
{
    internal class AsyncAppenderModel : AppenderModel, IAcceptAppenderRef
    {
        public AsyncAppenderModel(XmlNode node, string name, int incomingReferences)
            : base(AppenderDescriptor.Async, node, name, incomingReferences)
        {
        }
    }
}
