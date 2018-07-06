// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    public class AcceptOnMatch : PropertyBase
    {
        private const string AcceptOnMatchName = "acceptOnMatch";

        public AcceptOnMatch(ReadOnlyCollection<IProperty> container)
            : base(container, GridLength.Auto)
        {
        }

        public bool Accept { get; set; } = true;

        public override void Load(XmlNode originalNode)
        {
            string acceptStr = originalNode.GetValueAttributeValueFromChildElement(AcceptOnMatchName);
            if (bool.TryParse(acceptStr, out bool accept))
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
