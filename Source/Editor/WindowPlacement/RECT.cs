// Copyright © 2018 Alex Leendertsen

using System;
using System.Runtime.InteropServices;

namespace Editor.WindowPlacement
{
    /// <summary>
    /// Structure required by <see cref="WINDOWPLACEMENT"/> structure.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    // ReSharper disable once InconsistentNaming
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }
}
