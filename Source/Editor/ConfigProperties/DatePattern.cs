// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    public class DatePattern : StringValueProperty
    {
        private const string DatePatternName = "datePattern";

        public DatePattern(ReadOnlyCollection<IProperty> container)
            : base(container, GridLength.Auto, "Date Pattern:")
        {
            ToolTip = "This property determines the rollover schedule when rolling over on date.";
        }

        public override void Load(XmlNode originalNode)
        {
            SetValueIfNotNullOrEmpty(originalNode.GetValueAttributeValueFromChildElement(DatePatternName));
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            if (string.IsNullOrEmpty(Value))
            {
                messageBoxService.ShowError("A valid date pattern must be assigned.");
                return false;
            }

            return base.TryValidate(messageBoxService);
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            xmlDoc.CreateElementWithValueAttribute(DatePatternName, Value).AppendTo(newNode);
        }
    }
}
