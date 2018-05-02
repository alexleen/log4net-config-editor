// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;

namespace Editor.Windows.Appenders.Properties
{
    public class LogName : StringValueProperty
    {
        public LogName(ObservableCollection<IAppenderProperty> container, XmlNode appenderNode)
            : base(container, GridLength.Auto, "Log Name:", appenderNode)
        {
        }

        public override void Load()
        {
            SetValueIfNotNullOrEmpty(AppenderNode.SelectSingleNode("param[@name='LogName']")?.Attributes?["value"]?.Value);
        }

        public override bool TryValidate()
        {
            if (string.IsNullOrEmpty(Value))
            {
                MessageBox.Show("A log name must be assigned to this appender.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return base.TryValidate();
        }

        public override void Save(XmlDocument xmlDoc)
        {
            xmlDoc.CreateElementWithAttributes("param",
                                               new[]
                                               {
                                                   (Name: "name", Value: "LogName"),
                                                   (Name: "value", Value: Value)
                                               })
                  .AppendTo(AppenderNode);
        }
    }
}
