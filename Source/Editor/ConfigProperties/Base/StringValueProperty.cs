// Copyright © 2020 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Utilities;

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

        public override void Load(XmlNode originalNode)
        {
            SetValueIfNotNullOrEmpty(originalNode[ElementName]?.Attributes[mAttributeName]?.Value);
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (!string.IsNullOrEmpty(Value))
            {
                xmlDoc.CreateElementWithAttribute(ElementName, mAttributeName, Value).AppendTo(newNode);
            }
        }
    }
}
