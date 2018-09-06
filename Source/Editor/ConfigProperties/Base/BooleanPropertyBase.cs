// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties.Base
{
    internal class BooleanPropertyBase : PropertyBase
    {
        private readonly string mElementName;
        private readonly bool mIgnoreValue;

        /// <summary>
        /// ctor for <see cref="BooleanPropertyBase"/>
        /// </summary>
        /// <param name="container">Collection of properties in which this property exists</param>
        /// <param name="name">Display name shown in the UI</param>
        /// <param name="elementName">Name of the element this property is read from and saved as</param>
        /// <param name="defaultValue">Default value for the <see cref="Value"/> property</param>
        /// <param name="ignoreValue">Property will not be saved if <see cref="Value"/> equals this value</param>
        public BooleanPropertyBase(ReadOnlyCollection<IProperty> container, string name, string elementName, bool defaultValue, bool ignoreValue)
            : base(container, GridLength.Auto)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            mElementName = elementName;
            mIgnoreValue = ignoreValue;
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
            if (Value != mIgnoreValue)
            {
                xmlDoc.CreateElementWithValueAttribute(mElementName, Value.ToString()).AppendTo(newNode);
            }
        }
    }
}
