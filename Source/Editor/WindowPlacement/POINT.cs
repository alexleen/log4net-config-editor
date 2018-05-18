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
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
