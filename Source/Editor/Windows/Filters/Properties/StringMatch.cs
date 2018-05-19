// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.Appenders.Properties;

namespace Editor.Windows.Filters.Properties
{
    public class StringMatch : StringValueProperty
    {
        private const string StringMatchName = "stringToMatch";

        public StringMatch(ObservableCollection<IAppenderProperty> container)
            : base(container, GridLength.Auto, "String to Match:")
        {
        }

        public override void Load(XmlNode originalAppenderNode)
        {
            SetValueIfNotNullOrEmpty(originalAppenderNode.GetValueAttributeValueFromChildElement(StringMatchName));
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

        public override void Save(XmlDocument xmlDoc, XmlNode newAppenderNode)
        {
            xmlDoc.CreateElementWithValueAttribute(StringMatchName, Value).AppendTo(newAppenderNode);
        }
    }
}
