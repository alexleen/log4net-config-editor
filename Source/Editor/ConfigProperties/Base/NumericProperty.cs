// Copyright © 2020 Alex Leendertsen

using System;
using System.Xml;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties.Base
{
    public class NumericProperty<T> : StringValueProperty
    {
        private readonly T mDefaultValue;

        internal NumericProperty(string name, string elementName, T defaultValue)
            : base(name, elementName)
        {
            mDefaultValue = defaultValue;

            if (!Equals(mDefaultValue, default(T)))
            {
                Value = mDefaultValue.ToString();
            }
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            try
            {
                Convert.ChangeType(Value, typeof(T));
            }
            catch
            {
                messageBoxService.ShowError($"'{Name.TrimEnd(':')}' must be a valid {typeof(T).Name}.");
                return false;
            }

            return base.TryValidate(messageBoxService);
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (!string.IsNullOrEmpty(Value))
            {
                T value = (T)Convert.ChangeType(Value, typeof(T));

                if (!Equals(value, mDefaultValue))
                {
                    xmlDoc.CreateElementWithValueAttribute(ElementName, value.ToString()).AppendTo(newNode);
                }
            }
        }
    }
}
