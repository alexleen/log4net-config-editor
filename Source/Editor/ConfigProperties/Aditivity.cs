// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    internal class Aditivity : BooleanPropertyBase
    {
        private const string ElementName = "aditivity";

        public Aditivity(ReadOnlyCollection<IProperty> container)
            : base(container, "Aditivity:", ElementName, true, true)
        {
        }

        public override void Load(XmlNode originalNode)
        {
            string valueStr = originalNode.Attributes[ElementName]?.Value;
            if (bool.TryParse(valueStr, out bool value))
            {
                Value = value;
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (!Value)
            {
                newNode.AppendAttribute(xmlDoc, ElementName, Value.ToString());
            }
        }
    }
}
