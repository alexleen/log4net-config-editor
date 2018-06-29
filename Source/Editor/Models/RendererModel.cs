using Editor.Descriptors;
using System.Xml;

namespace Editor.Models
{
    class RendererModel : ChildModel
    {
        public RendererModel(XmlNode node)
            : base(RendererDescriptor.Renderer.ElementName, node)
        {
        }
    }
}
