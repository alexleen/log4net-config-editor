// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    public class PreserveExtension : PropertyBase
    {
        private const string PreserveExtensionName = "preserveLogFileNameExtension";

        public PreserveExtension(ReadOnlyCollection<IProperty> container)
            : base(container, GridLength.Auto)
        {
        }

        public bool Preserve { get; set; }

        public override void Load(XmlNode originalNode)
        {
            string preserveStr = originalNode.GetValueAttributeValueFromChildElement(PreserveExtensionName);
            if (!string.IsNullOrEmpty(preserveStr) && bool.TryParse(preserveStr, out bool preserve))
            {
                Preserve = preserve;
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (Preserve)
            {
                xmlDoc.CreateElementWithValueAttribute(PreserveExtensionName, Preserve.ToString().ToLower()).AppendTo(newNode);
            }
        }
    }
}
