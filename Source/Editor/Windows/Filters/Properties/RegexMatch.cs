// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.Appenders.Properties;

namespace Editor.Windows.Filters.Properties
{
    public class RegexMatch : StringValueProperty
    {
        private const string RegexToMatchName = "regexToMatch";

        public RegexMatch(ObservableCollection<IAppenderProperty> container)
            : base(container, GridLength.Auto, "Regex to Match:")
        {
        }

        public override void Load(XmlNode originalAppenderNode)
        {
            SetValueIfNotNullOrEmpty(originalAppenderNode.GetValueAttributeValueFromChildElement(RegexToMatchName));
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

        public override void Save(XmlDocument xmlDoc, XmlNode newAppenderNode)
        {
            xmlDoc.CreateElementWithValueAttribute(RegexToMatchName, Value).AppendTo(newAppenderNode);
        }
    }
}
