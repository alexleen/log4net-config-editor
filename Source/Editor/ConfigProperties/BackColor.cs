using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;

namespace Editor.ConfigProperties
{
    internal class BackColor : ColorPropertyBase
    {
        internal BackColor(ReadOnlyCollection<IProperty> container)
            : base(container, GridLength.Auto, "Background:", "backColor")
        {
        }
    }
}
