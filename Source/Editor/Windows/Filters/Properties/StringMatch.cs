// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.Appenders.Properties;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Filters.Properties
{
    public class StringMatch : StringValueProperty
    {
        private const string StringMatchName = "stringToMatch";

        public StringMatch(ObservableCollection<IProperty> container)
            : base(container, GridLength.Auto, "String to Match:")
        {
        }

        public override void Load(XmlNode originalNode)
        {
            SetValueIfNotNullOrEmpty(originalNode.GetValueAttributeValueFromChildElement(StringMatchName));
        }

        public override bool TryValidate()
        {
            if (string.IsNullOrEmpty(Value))
            {
                MessageBox.Show("'String to Match' must be specified.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return base.TryValidate();
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            xmlDoc.CreateElementWithValueAttribute(StringMatchName, Value).AppendTo(newNode);
        }
    }
}
