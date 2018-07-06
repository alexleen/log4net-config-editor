// Copyright © 2018 Alex Leendertsen

using System.Windows;
using Size = Editor.Windows.SizeLocation.Size;

namespace Editor.Interfaces
{
    /// <summary>
    /// Represents various sizing and location aspects of a window.
    /// </summary>
    internal interface IWindowSizeLocation
    {
        /// <summary>
        /// The key used to store this window's location in Settings.Default.
        /// Null if location is not to be saved.
        /// </summary>
        string RetentionKey { get; }

        /// <summary>
        /// Resize mode
        /// </summary>
        ResizeMode ResizeMode { get; }

        /// <summary>
        /// Gets a value that indicates whether a window will automatically size itself to fit the size of its content.
        /// </summary>
        SizeToContent SizeToContent { get; }

        /// <summary>
        /// Width information
        /// </summary>
        Size Width { get; }

        /// <summary>
        /// Height information
        /// </summary>
        Size Height { get; }
    }
}
