// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Appenders.Properties
{
    public class DatePattern : StringValueProperty
    {
        private const string DatePatternName = "datePattern";

        public DatePattern(ObservableCollection<IProperty> container)
            : base(container, GridLength.Auto, "Date Pattern:")
        {
            ToolTip = "This property determines the rollover schedule when rolling over on date.";
        }

        public override void Load(XmlNode originalNode)
        {
            SetValueIfNotNullOrEmpty(originalNode.GetValueAttributeValueFromChildElement(DatePatternName));
        }

        public override bool TryValidate()
        {
            if (string.IsNullOrEmpty(Value))
            {
                MessageBox.Show("A valid date pattern must be assigned.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return base.TryValidate();
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            xmlDoc.CreateElementWithValueAttribute(DatePatternName, Value).AppendTo(newNode);
        }
    }
}
