// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Filters.Properties
{
    public class StringMatch : StringValueProperty
    {
        private readonly Func<bool> mValidate;
        private const string StringMatchName = "stringToMatch";

        public StringMatch(ObservableCollection<IProperty> container, Func<bool> validate)
            : base(container, GridLength.Auto, "String to Match:")
        {
            mValidate = validate;
        }

        public override void Load(XmlNode originalNode)
        {
            SetValueIfNotNullOrEmpty(originalNode.GetValueAttributeValueFromChildElement(StringMatchName));
        }

        public override bool TryValidate()
        {
            return mValidate();
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            xmlDoc.CreateElementWithValueAttribute(StringMatchName, Value).AppendTo(newNode);
        }
    }
}
