using Editor.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Editor.ConfigProperties.Base
{
    internal abstract class EnumPropertyBase<TEnumType> : MultiValuePropertyBase<string>
    {
        protected EnumPropertyBase(ReadOnlyCollection<IProperty> container, GridLength rowHeight, string name, double width)
            : base(container, rowHeight, name, Enum.GetNames(typeof(TEnumType)), width)
        {
        }
    }
}
