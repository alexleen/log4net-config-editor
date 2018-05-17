// Copyright © 2018 Alex Leendertsen

using System;
using System.Runtime.InteropServices;

namespace Editor.WindowPlacement
{
    /// <summary>
    /// Stores the position, size, and state of a window.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    // ReSharper disable once InconsistentNaming
    public struct WINDOWPLACEMENT
    {
        public int length;
        public int flags;
        public int showCmd;
        public POINT minPosition;
        public POINT maxPosition;
        public RECT normalPosition;
    }
}
