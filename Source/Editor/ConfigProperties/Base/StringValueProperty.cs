// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Utilities;

namespace Editor.ConfigProperties.Base
{
    public class StringValueProperty : PropertyBase
    {
        private readonly string mElementName;

        internal StringValueProperty(string name, string elementName)
            : base(GridLength.Auto)
        {
            mElementName = elementName;
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
            SetValueIfNotNullOrEmpty(originalNode.GetValueAttributeValueFromChildElement(mElementName));
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (!string.IsNullOrEmpty(Value))
            {
                xmlDoc.CreateElementWithValueAttribute(mElementName, Value).AppendTo(newNode);
            }
        }
    }
}
