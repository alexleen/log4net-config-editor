// Copyright © 2020 Alex Leendertsen

using System;
using System.Windows;
using Editor.Interfaces;
using Editor.Utilities;
using Editor.XML;

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

        public override void Load(IElementConfiguration config)
        {
            if (config.Load(Log4NetXmlConstants.Value, out IValueResult result, mElementName) && bool.TryParse(result.AttributeValue, out bool value))
            {
                Value = value;
            }
        }

        public override void Save(IElementConfiguration config)
        {
            if (Value != mDefaultValue)
            {
                config.Save(new Element(mElementName, new[] { (Log4NetXmlConstants.Value, Value.ToString()) }));
            }
        }
    }
}
