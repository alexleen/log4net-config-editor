// Copyright © 2018 Alex Leendertsen

using System.Xml;
using Editor.Descriptors.Base;

namespace Editor.Models.ConfigChildren
{
    internal class RootLoggerModel : LoggerModel
    {
        public RootLoggerModel(XmlNode node, bool isEnabled, DescriptorBase descriptor)
            : base(node, isEnabled, descriptor)
        {
        }

        public override string Name => Node.Name;
    }
}
