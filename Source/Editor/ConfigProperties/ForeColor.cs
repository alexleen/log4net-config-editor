using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;

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
