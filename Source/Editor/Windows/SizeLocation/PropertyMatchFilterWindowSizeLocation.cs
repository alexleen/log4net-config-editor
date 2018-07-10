// Copyright © 2018 Alex Leendertsen

using System.Windows;

namespace Editor.Windows.SizeLocation
{
    internal class PropertyMatchFilterWindowSizeLocation : FilterWindowSizeLocation
    {
        public override ResizeMode ResizeMode { get; } = ResizeMode.CanResize;
        public override Size Width { get; } = new Size(min: 350);
        public override Size Height { get; } = new Size(min: 173, max: 173);
    }
}
