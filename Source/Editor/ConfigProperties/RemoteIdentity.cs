// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Utilities;
using Editor.XML;

namespace Editor.ConfigProperties
{
    internal class RemoteIdentity : StringValueProperty
    {
        public RemoteIdentity()
            : base("Identity:", Log4NetXmlConstants.Identity)
        {
            ToolTip = "Enter remote syslog identity pattern here.";
        }

        public override void Save(IElementConfiguration config)
        {
            if (!string.IsNullOrEmpty(Value))
            {
                config.Save(new Element(Log4NetXmlConstants.Identity, new[] { (Log4NetXmlConstants.Value, Value), (Log4NetXmlConstants.Type, LayoutDescriptor.Pattern.TypeNamespace) }));
            }
        }
    }
}
