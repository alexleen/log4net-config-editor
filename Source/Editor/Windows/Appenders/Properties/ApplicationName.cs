// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;

namespace Editor.Windows.Appenders.Properties
{
    public class ApplicationName : StringValueProperty
    {
        public ApplicationName(ObservableCollection<IAppenderProperty> container, XmlNode appenderNode)
            : base(container, GridLength.Auto, "Application Name:", appenderNode)
        {
        }

        public override void Load()
        {
            SetValueIfNotNullOrEmpty(AppenderNode.SelectSingleNode("param[@name='ApplicationName']")?.Attributes?["value"]?.Value);
        }

        public override bool TryValidate()
        {
            if (string.IsNullOrEmpty(Value))
            {
                MessageBox.Show("An application name must be assigned to this appender.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return base.TryValidate();
        }

        public override void Save(XmlDocument xmlDoc)
        {
            xmlDoc.CreateElementWithAttributes("param",
                                               new[]
                                               {
                                                   (Name: "name", Value: "ApplicationName"),
                                                   (Name: "value", Value: Value)
                                               })
                  .AppendTo(AppenderNode);
        }
    }
}
