// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.Appenders.Properties;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Filters.Properties
{
    public class AcceptOnMatch : PropertyBase
    {
        private const string AcceptOnMatchName = "acceptOnMatch";

        public AcceptOnMatch(ObservableCollection<IProperty> container)
            : base(container, GridLength.Auto)
        {
        }

        public bool Accept { get; set; } = true;

        public override void Load(XmlNode originalNode)
        {
            string acceptStr = originalNode.GetValueAttributeValueFromChildElement(AcceptOnMatchName);
            if (!string.IsNullOrEmpty(acceptStr) && bool.TryParse(acceptStr, out bool accept))
            {
                Accept = accept;
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (!Accept)
            {
                xmlDoc.CreateElementWithValueAttribute(AcceptOnMatchName, "false").AppendTo(newNode);
            }
        }
    }
}
