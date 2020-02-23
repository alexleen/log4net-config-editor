// Copyright © 2018 Alex Leendertsen

using System.Net;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    internal class Port : StringValueProperty
    {
        internal Port(string name, string elementName)
            : base(name, elementName)
        {
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
    }
}
