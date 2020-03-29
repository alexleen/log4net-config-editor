// Copyright © 2020 Alex Leendertsen

using System.Net;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    internal class Port : StringValueProperty
    {
        private readonly string mDefaultPort;

        internal Port(string name, string elementName, int? defaultPort)
            : base(name, elementName)
        {
            mDefaultPort = defaultPort?.ToString();
            Value = mDefaultPort;
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            if (!int.TryParse(Value, out int valueInt) || valueInt < IPEndPoint.MinPort || valueInt > IPEndPoint.MaxPort)
            {
                messageBoxService.ShowError($"Port must be a valid integer between {IPEndPoint.MinPort} and {IPEndPoint.MaxPort}.");
                return false;
            }

            return base.TryValidate(messageBoxService);
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (!string.IsNullOrEmpty(Value) && Value != mDefaultPort)
            {
                xmlDoc.CreateElementWithValueAttribute(ElementName, Value).AppendTo(newNode);
            }
        }
    }
}
