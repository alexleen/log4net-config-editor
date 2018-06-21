// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Appenders.Properties
{
    public class CountDirection : PropertyBase
    {
        private const string Lower = "Lower";
        private const string Higher = "Higher";
        private const string CountDirectionName = "countDirection";

        public CountDirection(ObservableCollection<IProperty> container)
            : base(container, GridLength.Auto)
        {
            Directions = new[] { Lower, Higher };
            SelectedDirection = Lower;
        }

        public IEnumerable<string> Directions { get; }

        public string SelectedDirection { get; set; }

        public override void Load(XmlNode originalNode)
        {
            if (int.TryParse(originalNode.GetValueAttributeValueFromChildElement(CountDirectionName), out int value))
            {
                SelectedDirection = value >= 0 ? Higher : Lower;
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (SelectedDirection == Higher)
            {
                xmlDoc.CreateElementWithValueAttribute(CountDirectionName, 0.ToString()).AppendTo(newNode);
            }
        }
    }
}
