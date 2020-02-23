// Copyright © 2018 Alex Leendertsen

using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    internal class Value : StringValueProperty
    {
        private const string ValueName = "value";

        internal Value()
            : base("Value:", null)
        {
        }

        public override void Load(XmlNode originalNode)
        {
            SetValueIfNotNullOrEmpty(originalNode.Attributes[ValueName]?.Value);
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (!string.IsNullOrEmpty(Value))
            {
                newNode.AppendAttribute(xmlDoc, ValueName, Value);
            }
        }
    }
}
