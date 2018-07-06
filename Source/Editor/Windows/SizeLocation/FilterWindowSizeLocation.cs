// Copyright © 2018 Alex Leendertsen

using System.Windows;
using Editor.Interfaces;

namespace Editor.Windows.SizeLocation
{
    internal class FilterWindowSizeLocation : IWindowSizeLocation
    {
        public string RetentionKey { get; } = null;
        public virtual ResizeMode ResizeMode { get; } = ResizeMode.NoResize;
        public SizeToContent SizeToContent { get; } = SizeToContent.WidthAndHeight;
        public virtual Size Width { get; } = new Size();
        public virtual Size Height { get; } = new Size();
    }
}
