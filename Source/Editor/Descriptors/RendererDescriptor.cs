// Copyright © 2019 Alex Leendertsen

using Editor.Descriptors.Base;
using Editor.Utilities;

namespace Editor.Descriptors
{
    public class RendererDescriptor : DescriptorBase
    {
        public static readonly RendererDescriptor Renderer;

        static RendererDescriptor()
        {
            Renderer = new RendererDescriptor("Renderer");
        }

        private RendererDescriptor(string name)
            : base(name, Log4NetXmlConstants.Renderer, null)
        {
        }
    }
}
