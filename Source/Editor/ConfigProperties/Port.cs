// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Net;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    internal abstract class Port : StringValueProperty
    {
        protected Port(ReadOnlyCollection<IProperty> container, string name, string elementName)
            : base(container, name, elementName)
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
