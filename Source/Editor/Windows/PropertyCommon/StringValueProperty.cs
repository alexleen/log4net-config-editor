// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;

namespace Editor.Windows.PropertyCommon
{
    public abstract class StringValueProperty : PropertyBase
    {
        protected StringValueProperty(ObservableCollection<IProperty> container, GridLength rowHeight, string name)
            : base(container, rowHeight)
        {
            Name = name;
        }

        public string Name { get; }

        public string Value { get; set; }

        public bool IsFocused { get; set; }

        public string ToolTip { get; set; }

        protected void SetValueIfNotNullOrEmpty(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Value = value;
            }
        }
    }
}
