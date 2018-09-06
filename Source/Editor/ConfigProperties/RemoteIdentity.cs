// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    internal class RemoteIdentity : StringValueProperty
    {
        public RemoteIdentity(ReadOnlyCollection<IProperty> container)
            : base(container, "Identity:", Log4NetXmlConstants.Identity)
        {
            ToolTip = "Enter remote syslog identity pattern here.";
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (!string.IsNullOrEmpty(Value))
            {
                xmlDoc.CreateElementWithAttributes(Log4NetXmlConstants.Identity, new[] { (Log4NetXmlConstants.Value, Value), (Log4NetXmlConstants.Type, LayoutDescriptor.Pattern.TypeNamespace) }).AppendTo(newNode);
            }
        }
    }
}
