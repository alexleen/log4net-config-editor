// Copyright © 2020 Alex Leendertsen

using System.Linq;
using System.Windows;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    internal class TypeAttribute : MultiValuePropertyBase<AppenderDescriptor>
    {
        private const double WidthValue = 350;
        private const string TypeName = "type";

        private static readonly AppenderDescriptor[] sValues =
        {
            AppenderDescriptor.AspNetTrace,
            AppenderDescriptor.Async,
            AppenderDescriptor.BufferingForwarding,
            AppenderDescriptor.Console,
            AppenderDescriptor.Debug,
            AppenderDescriptor.EventLog,
            AppenderDescriptor.File,
            AppenderDescriptor.Forwarding,
            AppenderDescriptor.LocalSyslog,
            AppenderDescriptor.ManagedColor,
            AppenderDescriptor.Memory,
            AppenderDescriptor.NetSend,
            AppenderDescriptor.OutputDebugString,
            AppenderDescriptor.RemoteSyslog,
            AppenderDescriptor.Remoting,
            AppenderDescriptor.RollingFile,
            AppenderDescriptor.Smtp,
            AppenderDescriptor.SmtpPickupDir,
            AppenderDescriptor.Telnet,
            AppenderDescriptor.TextWriter,
            AppenderDescriptor.Trace,
            AppenderDescriptor.Udp
        };

        public TypeAttribute()
            : base(GridLength.Auto, "Type:", sValues, WidthValue)
        {
            SelectedValue = sValues.First();
        }

        public TypeAttribute(AppenderDescriptor descriptor)
            : base(GridLength.Auto, "Type:", sValues, WidthValue)
        {
            //Will be overwritten when/if load is called
            SelectedValue = descriptor;
        }

        public override void Load(XmlNode originalNode)
        {
            if (AppenderDescriptor.TryFindByTypeNamespace(originalNode.Attributes[TypeName].Value, out AppenderDescriptor descriptor))
            {
                SelectedValue = descriptor;
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            newNode.AppendAttribute(xmlDoc, TypeName, SelectedValue.TypeNamespace);
        }
    }
}
