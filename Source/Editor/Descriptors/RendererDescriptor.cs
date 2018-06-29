// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors.Base;

namespace Editor.Descriptors
{
    public class RendererDescriptor : DescriptorBase
    {
        public static readonly RendererDescriptor Renderer;

        static RendererDescriptor()
        {
            Renderer = new RendererDescriptor("Renderer");
        }

        public RendererDescriptor(string name)
            : base(name, "renderer")
        {
        }
    }
}
