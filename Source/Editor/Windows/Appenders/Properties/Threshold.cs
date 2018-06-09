// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.Filters.Properties;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Appenders.Properties
{
    public class Threshold : LevelPropertyBase
    {
        private const string ThresholdName = "threshold";

        public Threshold(ObservableCollection<IProperty> container)
            : base(container, GridLength.Auto, "Threshold:", true)
        {
        }

        public override void Load(XmlNode originalNode)
        {
            TryLoadSelectedLevel(originalNode.GetValueAttributeValueFromChildElement(ThresholdName));
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (!string.IsNullOrEmpty(SelectedLevel))
            {
                xmlDoc.CreateElementWithValueAttribute(ThresholdName, SelectedLevel).AppendTo(newNode);
            }
        }
    }
}
