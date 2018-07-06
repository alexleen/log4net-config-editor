// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    internal class ForeColor : ColorPropertyBase
    {
        internal ForeColor(ReadOnlyCollection<IProperty> container)
            : base(container, GridLength.Auto, "Foreground:", "foreColor")
        {
        }
    }
}
