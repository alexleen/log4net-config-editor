// Copyright © 2018 Alex Leendertsen

using System.Windows;
using Editor.Interfaces;

namespace Editor.Windows.SizeLocation
{
    internal class AppenderWindowSizeLocation : IWindowSizeLocation
    {
        public string RetentionKey { get; } = "AppenderWindowPlacement";
        public ResizeMode ResizeMode { get; } = ResizeMode.CanResize;
        public SizeToContent SizeToContent { get; } = SizeToContent.Manual;
        public Size Width { get; } = new Size(550, min: 550);
        public Size Height { get; } = new Size(500, min: 500);
    }
}
