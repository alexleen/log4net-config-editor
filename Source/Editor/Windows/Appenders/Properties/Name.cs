// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Appenders.Properties
{
    public class Name : StringValueProperty
    {
        public Name(ObservableCollection<IProperty> container)
            : base(container, GridLength.Auto, "Name:")
        {
            IsFocused = true;
        }

        public override void Load(XmlNode originalNode)
        {
            SetValueIfNotNullOrEmpty(originalNode.Attributes?["name"]?.Value);
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            //TODO name uniqueness
            if (string.IsNullOrEmpty(Value))
            {
                MessageBox.Show("A name must be assigned to this appender.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return base.TryValidate(messageBoxService);
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            newNode.AppendAttribute(xmlDoc, "name", Value);
        }
    }
}
