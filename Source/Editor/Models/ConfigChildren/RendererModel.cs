// Copyright © 2018 Alex Leendertsen

using System.Xml;
using Editor.Descriptors;
using Editor.Models.Base;

namespace Editor.Models.ConfigChildren
{
    internal class RendererModel : ModelBase
    {
        public RendererModel(XmlNode node)
            : base(node, RendererDescriptor.Renderer)
        {
        }
    }
}
