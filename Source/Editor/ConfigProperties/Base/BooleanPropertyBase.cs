// Copyright © 2020 Alex Leendertsen

using System;
using System.Windows;
using System.Xml;
using Editor.Utilities;

namespace Editor.ConfigProperties.Base
{
    internal class BooleanPropertyBase : PropertyBase
    {
        private readonly bool mDefaultValue;
        private readonly string mElementName;

        /// <summary>
        ///     ctor for <see cref="BooleanPropertyBase" />
        /// </summary>
        /// <param name="name">Display name shown in the UI</param>
        /// <param name="elementName">Name of the element this property is read from and saved as</param>
        /// <param name="defaultValue">Default value for the <see cref="Value" /> property. This value is not saved.</param>
        public BooleanPropertyBase(string name, string elementName, bool defaultValue)
            : base(GridLength.Auto)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            mElementName = elementName ?? throw new ArgumentNullException(nameof(elementName));
            mDefaultValue = defaultValue;
            Value = defaultValue;
        }

        public string Name { get; }

        public bool Value { get; set; }

        public override void Load(XmlNode originalNode)
        {
            string valueStr = originalNode.GetValueAttributeValueFromChildElement(mElementName);
            if (bool.TryParse(valueStr, out bool value))
            {
                Value = value;
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (Value != mDefaultValue)
            {
                xmlDoc.CreateElementWithValueAttribute(mElementName, Value.ToString()).AppendTo(newNode);
            }
        }
    }
}
