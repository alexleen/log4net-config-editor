using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Xml;

namespace Editor.ConfigProperties
{
    internal abstract class Port : StringValueProperty
    {
        protected Port(ReadOnlyCollection<IProperty> container, string name)
            : base(container, GridLength.Auto, name)
        {
        }

        public override void Load(XmlNode originalNode)
        {
            throw new NotImplementedException();
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            if (!int.TryParse(Value, out int valueInt) || (valueInt < IPEndPoint.MinPort || valueInt > IPEndPoint.MaxPort))
            {
                messageBoxService.ShowError($"Port must be a valid integer between {IPEndPoint.MinPort} and {IPEndPoint.MaxPort}.");
                return false;
            }

            return base.TryValidate(messageBoxService);
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            throw new NotImplementedException();
        }
    }

    internal class RemotePort : Port
    {
        internal RemotePort(ReadOnlyCollection<IProperty> container)
            : base(container, "Remote Port:")
        {
        }
    }

    internal class LocalPort : Port
    {
        internal LocalPort(ReadOnlyCollection<IProperty> container)
            : base(container, "Local Port:")
        {
        }
    }
}
