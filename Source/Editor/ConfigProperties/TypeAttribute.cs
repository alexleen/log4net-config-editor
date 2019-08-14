// Copyright © 2019 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    public class TypeAttribute : MultiValuePropertyBase<AppenderDescriptor>
    {
        private const double WidthValue = 350;
        private const string TypeName = "type";

        private static readonly AppenderDescriptor[] sValues =
        {
            AppenderDescriptor.Console,
            AppenderDescriptor.File,
            AppenderDescriptor.RollingFile,
            AppenderDescriptor.EventLog,
            AppenderDescriptor.Async,
            AppenderDescriptor.Forwarding,
            AppenderDescriptor.ManagedColor,
            AppenderDescriptor.Udp,
            AppenderDescriptor.LocalSyslog,
            AppenderDescriptor.RemoteSyslog
        };

        public TypeAttribute(ReadOnlyCollection<IProperty> container)
            : base(container, GridLength.Auto, "Type:", sValues, WidthValue)
        {
            SelectedValue = sValues.First();
        }

        public TypeAttribute(ReadOnlyCollection<IProperty> container, AppenderDescriptor descriptor)
            : base(container, GridLength.Auto, "Type:", sValues, WidthValue)
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
