// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors;
using Editor.Models.Base;
using System.Xml;

namespace Editor.Models
{
    internal class RendererModel : ModelBase
    {
        public RendererModel(XmlNode node)
            : base(node, RendererDescriptor.Renderer)
        {
        }
    }
}
