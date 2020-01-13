// Copyright © 2020 Alex Leendertsen

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

        private TValueType mSelectedValue;

        public TValueType SelectedValue
        {
            get => mSelectedValue;
            set
            {
                if (Equals(value, mSelectedValue))
                {
                    return;
                }

                mSelectedValue = value;
                OnPropertyChanged();
            }
        }

        public double Width { get; }
    }
}
