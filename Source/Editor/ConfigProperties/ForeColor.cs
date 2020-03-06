// Copyright © 2018 Alex Leendertsen

using System.Windows;
using Editor.ConfigProperties.Base;

namespace Editor.ConfigProperties
{
    internal class ForeColor : ColorPropertyBase
    {
        internal ForeColor()
            : base(GridLength.Auto, "Foreground:", "foreColor")
        {
        }
    }
}
