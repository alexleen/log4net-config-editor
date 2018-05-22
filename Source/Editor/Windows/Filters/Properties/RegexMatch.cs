// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Filters.Properties
{
    public class RegexMatch : StringValueProperty
    {
        private readonly Func<bool> mValidate;
        private const string RegexToMatchName = "regexToMatch";

        public RegexMatch(ObservableCollection<IProperty> container, Func<bool> validate)
            : base(container, GridLength.Auto, "Regex to Match:")
        {
            mValidate = validate;
        }

        public override void Load(XmlNode originalNode)
        {
            SetValueIfNotNullOrEmpty(originalNode.GetValueAttributeValueFromChildElement(RegexToMatchName));
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            return mValidate();
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            xmlDoc.CreateElementWithValueAttribute(RegexToMatchName, Value).AppendTo(newNode);
        }
    }
}
