// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    public class StringMatch : StringValueProperty
    {
        private readonly Func<bool> mValidate;
        private const string StringMatchName = "stringToMatch";

        public StringMatch(ReadOnlyCollection<IProperty> container, Func<bool> validate)
            : base(container, GridLength.Auto, "String to Match:")
        {
            mValidate = validate;
        }

        public override void Load(XmlNode originalNode)
        {
            SetValueIfNotNullOrEmpty(originalNode.GetValueAttributeValueFromChildElement(StringMatchName));
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            return mValidate();
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (!string.IsNullOrEmpty(Value))
            {
                xmlDoc.CreateElementWithValueAttribute(StringMatchName, Value).AppendTo(newNode);
            }
        }
    }
}
