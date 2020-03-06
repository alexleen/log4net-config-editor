// Copyright © 2020 Alex Leendertsen

using System.Collections.Generic;
using System.Windows;

namespace Editor.ConfigProperties.Base
{
    internal abstract class MultiValuePropertyBase<TValueType> : PropertyBase
    {
        private TValueType mSelectedValue;

        protected MultiValuePropertyBase(GridLength rowHeight, string name, IEnumerable<TValueType> values, double width)
            : base(rowHeight)
        {
            Name = name;
            Values = values;
            Width = width;
        }

        public string Name { get; }

        public IEnumerable<TValueType> Values { get; }

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
