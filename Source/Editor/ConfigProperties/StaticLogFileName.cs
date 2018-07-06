// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    public class StaticLogFileName : PropertyBase
    {
        private const string StaticLogFileNameName = "staticLogFileName";

        public StaticLogFileName(ReadOnlyCollection<IProperty> container)
            : base(container, GridLength.Auto)
        {
        }

        public bool Value { get; set; }

        public override void Load(XmlNode originalNode)
        {
            string valueStr = originalNode.GetValueAttributeValueFromChildElement(StaticLogFileNameName);
            if (bool.TryParse(valueStr, out bool value))
            {
                Value = value;
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            //Default is off - only save if on
            if (Value)
            {
                xmlDoc.CreateElementWithValueAttribute(StaticLogFileNameName, Value.ToString().ToLower()).AppendTo(newNode);
            }
        }
    }
}
