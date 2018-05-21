// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Filters.Properties
{
    public class Key : StringValueProperty
    {
        private const string KeyName = "key";

        public Key(ObservableCollection<IProperty> container)
            : base(container, GridLength.Auto, "Key:")
        {
        }

        public override void Load(XmlNode originalNode)
        {
            SetValueIfNotNullOrEmpty(originalNode.GetValueAttributeValueFromChildElement(KeyName));
        }

        public override bool TryValidate()
        {
            if (string.IsNullOrEmpty(Value))
            {
                MessageBox.Show("'Key' must be specified.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return base.TryValidate();
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            xmlDoc.CreateElementWithValueAttribute(KeyName, Value).AppendTo(newNode);
        }
    }
}
