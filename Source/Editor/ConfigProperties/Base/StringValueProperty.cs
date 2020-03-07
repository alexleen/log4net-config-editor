// Copyright © 2020 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Utilities;

namespace Editor.ConfigProperties.Base
{
    public class StringValueProperty : PropertyBase
    {
        protected readonly string ElementName;

        internal StringValueProperty(string name, string elementName)
            : base(GridLength.Auto)
        {
            ElementName = elementName;
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

        public override void Load(XmlNode originalNode)
        {
            SetValueIfNotNullOrEmpty(originalNode.GetValueAttributeValueFromChildElement(ElementName));
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (!string.IsNullOrEmpty(Value))
            {
                xmlDoc.CreateElementWithValueAttribute(ElementName, Value).AppendTo(newNode);
            }
        }
    }
}
