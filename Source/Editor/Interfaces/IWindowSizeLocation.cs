// Copyright © 2018 Alex Leendertsen

using System.Windows;

namespace Editor.Interfaces
{
    internal interface IWindowSizeLocation
    {
        string RetentionKey { get; }

        ResizeMode ResizeMode { get; }

        SizeToContent SizeToContent { get; }

        Size Width { get; }

        Size Height { get; }
    }
}
