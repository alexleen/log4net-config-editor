// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    public class LoggerToMatch : StringValueProperty
    {
        private const string LoggerMatchName = "loggerToMatch";

        public LoggerToMatch(ReadOnlyCollection<IProperty> container)
            : base(container, GridLength.Auto, "Logger to Match:")
        {
        }

        public override void Load(XmlNode originalNode)
        {
            SetValueIfNotNullOrEmpty(originalNode.GetValueAttributeValueFromChildElement(LoggerMatchName));
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            if (string.IsNullOrEmpty(Value))
            {
                messageBoxService.ShowError("'Logger to Match' must be specified.");
                return false;
            }

            return base.TryValidate(messageBoxService);
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (!string.IsNullOrEmpty(Value))
            {
                xmlDoc.CreateElementWithValueAttribute(LoggerMatchName, Value).AppendTo(newNode);
            }
        }
    }
}
