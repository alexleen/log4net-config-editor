using Editor.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Editor.ConfigProperties.Base
{
    internal abstract class EnumPropertyBase<TEnumType> : MultiValuePropertyBase<string>
    {
        protected EnumPropertyBase(ReadOnlyCollection<IProperty> container, GridLength rowHeight, double width)
            : base(container, rowHeight, Enum.GetNames(typeof(TEnumType)), width)
        {
        }
    }
}
