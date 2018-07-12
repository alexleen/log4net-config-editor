// Copyright © 2018 Alex Leendertsen

using System.Windows;
using Editor.Interfaces;

namespace Editor.Windows.SizeLocation
{
    internal class LoggerWindowSizeLocation : IWindowSizeLocation
    {
        public string RetentionKey { get; } = null;
        public ResizeMode ResizeMode { get; } = ResizeMode.CanResize;
        public SizeToContent SizeToContent { get; } = SizeToContent.Manual;
        public Size Width { get; } = new Size(360, min: 360);
        public Size Height { get; } = new Size(350, min: 350);
    }
}
