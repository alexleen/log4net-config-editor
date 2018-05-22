// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Appenders.Properties
{
    public class LogName : StringValueProperty
    {
        public LogName(ObservableCollection<IProperty> container)
            : base(container, GridLength.Auto, "Log Name:")
        {
        }

        public override void Load(XmlNode originalNode)
        {
            SetValueIfNotNullOrEmpty(originalNode.SelectSingleNode("param[@name='LogName']")?.Attributes?["value"]?.Value);
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            if (string.IsNullOrEmpty(Value))
            {
                messageBoxService.ShowError("A log name must be assigned to this appender.");
                return false;
            }

            return base.TryValidate(messageBoxService);
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            xmlDoc.CreateElementWithAttributes("param",
                                               new[]
                                               {
                                                   (Name: "name", Value: "LogName"),
                                                   (Name: "value", Value: Value)
                                               })
                  .AppendTo(newNode);
        }
    }
}
