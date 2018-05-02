// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;

namespace Editor.Windows.Appenders.Properties
{
    public class Name : StringValueProperty
    {
        public Name(ObservableCollection<IAppenderProperty> container, XmlNode appenderNode)
            : base(container, GridLength.Auto, "Name:", appenderNode)
        {
        }

        public override void Load()
        {
            SetValueIfNotNullOrEmpty(AppenderNode.Attributes?["name"]?.Value);
        }

        public override bool TryValidate()
        {
            //TODO name uniqueness
            if (string.IsNullOrEmpty(Value))
            {
                MessageBox.Show("A name must be assigned to this appender.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return base.TryValidate();
        }

        public override void Save(XmlDocument xmlDoc)
        {
            AppenderNode.AppendAttribute(xmlDoc, "name", Value);
        }
    }
}
