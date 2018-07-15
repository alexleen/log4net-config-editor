// Copyright © 2018 Alex Leendertsen

using System.Xml;
using Editor.Descriptors;

namespace Editor.Models
{
    internal class RendererModel : ChildModel
    {
        public RendererModel(XmlNode node)
            : base(RendererDescriptor.Renderer.ElementName, node)
        {
        }
    }
}
