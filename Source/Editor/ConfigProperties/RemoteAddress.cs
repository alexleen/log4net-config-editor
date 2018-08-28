using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Xml;

namespace Editor.ConfigProperties
{
    internal class RemoteAddress : StringValueProperty
    {
        internal RemoteAddress(ReadOnlyCollection<IProperty> container)
            : base(container, GridLength.Auto, "Remote Address:")
        {
        }

        public override void Load(XmlNode originalNode)
        {
            throw new NotImplementedException();
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            if (!IPAddress.TryParse(Value, out _))
            {
                messageBoxService.ShowError("Remote address must be a valid IP address.");
                return false;
            }

            return base.TryValidate(messageBoxService);
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            throw new NotImplementedException();
        }
    }
}
