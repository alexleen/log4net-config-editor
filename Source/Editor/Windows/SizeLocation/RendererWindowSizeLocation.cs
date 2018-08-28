// Copyright © 2018 Alex Leendertsen

using System.Windows;

namespace Editor.Windows.SizeLocation
{
    internal class RendererWindowSizeLocation : FilterWindowSizeLocation
    {
        public override ResizeMode ResizeMode { get; } = ResizeMode.CanResize;
        public override Size Width { get; } = new Size(min: 450);
        public override Size Height { get; } = new Size(min: 120, max: 120);
    }
}
