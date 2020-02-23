// Copyright © 2018 Alex Leendertsen

using System.Windows;
using Editor.ConfigProperties.Base;

namespace Editor.ConfigProperties
{
    internal class BackColor : ColorPropertyBase
    {
        internal BackColor()
            : base(GridLength.Auto, "Background:", "backColor")
        {
        }
    }
}
