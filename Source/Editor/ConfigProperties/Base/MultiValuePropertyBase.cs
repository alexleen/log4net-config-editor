// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Editor.Interfaces;

namespace Editor.ConfigProperties.Base
{
    internal abstract class MultiValuePropertyBase<TValueType> : PropertyBase
    {
        protected MultiValuePropertyBase(ReadOnlyCollection<IProperty> container, GridLength rowHeight, string name, IEnumerable<TValueType> values, double width)
            : base(container, rowHeight)
        {
            Name = name;
            Values = values;
            Width = width;
        }

        public string Name { get; }

        public IEnumerable<TValueType> Values { get; }

        public TValueType SelectedValue { get; set; }

        public double Width { get; }
    }
}
