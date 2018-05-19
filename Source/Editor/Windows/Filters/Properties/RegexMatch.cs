// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.Appenders.Properties;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Filters.Properties
{
    public class RegexMatch : StringValueProperty
    {
        private const string RegexToMatchName = "regexToMatch";

        public RegexMatch(ObservableCollection<IProperty> container)
            : base(container, GridLength.Auto, "Regex to Match:")
        {
        }

        public override void Load(XmlNode originalNode)
        {
            SetValueIfNotNullOrEmpty(originalNode.GetValueAttributeValueFromChildElement(RegexToMatchName));
        }

        public override bool TryValidate()
        {
            if (string.IsNullOrEmpty(Value))
            {
                MessageBox.Show("'Regex to Match' must be specified.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return base.TryValidate();
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            xmlDoc.CreateElementWithValueAttribute(RegexToMatchName, Value).AppendTo(newNode);
        }
    }
}
