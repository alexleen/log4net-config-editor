// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;

namespace Editor.Windows.Appenders.Properties
{
    public abstract class StringValueProperty : AppenderPropertyBase
    {
        protected StringValueProperty(ObservableCollection<IAppenderProperty> container, GridLength rowHeight, string name, XmlNode appenderNode)
            : base(container, rowHeight, appenderNode)
        {
            Name = name;
        }

        public string Name { get; }

        public string Value { get; set; }

        protected void SetValueIfNotNullOrEmpty(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Value = value;
            }
        }
    }
}
