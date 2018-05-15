// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;

namespace Editor.Windows.Appenders.Properties
{
    public class CountDirection : AppenderPropertyBase
    {
        private const string Lower = "Lower";
        private const string Higher = "Higher";
        private const string CountDirectionName = "countDirection";

        public CountDirection(ObservableCollection<IAppenderProperty> container)
            : base(container, GridLength.Auto)
        {
            Directions = new[] { Lower, Higher };
            SelectedDirection = Lower;
        }

        public IEnumerable<string> Directions { get; }

        public string SelectedDirection { get; set; }

        public override void Load(XmlNode originalAppenderNode)
        {
            string valueStr = originalAppenderNode.GetValueAttributeValueFromChildElement(CountDirectionName);
            if (!string.IsNullOrEmpty(valueStr) && int.TryParse(valueStr, out int value))
            {
                SelectedDirection = value >= 0 ? Higher : Lower;
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newAppenderNode)
        {
            if (SelectedDirection == Higher)
            {
                xmlDoc.CreateElementWithAttribute(CountDirectionName, "value", 0.ToString()).AppendTo(newAppenderNode);
            }
        }
    }
}
