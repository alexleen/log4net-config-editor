// Copyright © 2020 Alex Leendertsen

using System.Windows;
using Editor.Interfaces;
using Editor.Utilities;
using Editor.XML;

namespace Editor.ConfigProperties.Base
{
    public class StringValueProperty : PropertyBase
    {
        protected readonly string ElementName;
        private readonly string mAttributeName;

        internal StringValueProperty(string name, string elementName, string attributeName = Log4NetXmlConstants.Value)
            : base(GridLength.Auto)
        {
            ElementName = elementName;
            mAttributeName = attributeName;
            Name = name;
        }

        public string Name { get; }

        public string Value { get; set; }

        public bool IsFocused { get; set; }

        public bool IsReadOnly { get; set; }

        protected void SetValueIfNotNullOrEmpty(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Value = value;
            }
        }

        public override void Load(IElementConfiguration config)
        {
            if (config.Load(mAttributeName, out IValueResult result, ElementName))
            {
                Value = result.AttributeValue;
            }
        }

        public override void Save(IElementConfiguration config)
        {
            if (!string.IsNullOrEmpty(Value))
            {
                config.Save(new Element(ElementName, new[] { (mAttributeName, Value) }));
            }
        }
    }
}
