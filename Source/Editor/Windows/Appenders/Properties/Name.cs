// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;

namespace Editor.Windows.Appenders.Properties
{
    public class Name : StringValueProperty
    {
        public Name(ObservableCollection<IAppenderProperty> container)
            : base(container, GridLength.Auto, "Name:")
        {
            IsFocused = true;
        }

        public override void Load(XmlNode originalAppenderNode)
        {
            SetValueIfNotNullOrEmpty(originalAppenderNode.Attributes?["name"]?.Value);
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

        public override void Save(XmlDocument xmlDoc, XmlNode newAppenderNode)
        {
            newAppenderNode.AppendAttribute(xmlDoc, "name", Value);
        }
    }
}
