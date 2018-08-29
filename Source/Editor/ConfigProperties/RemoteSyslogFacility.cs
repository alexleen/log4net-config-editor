// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using static log4net.Appender.RemoteSyslogAppender;

namespace Editor.ConfigProperties
{
    internal class RemoteSyslogFacility : EnumPropertyBase<SyslogFacility>
    {
        internal RemoteSyslogFacility(ReadOnlyCollection<IProperty> container)
            : base(container, GridLength.Auto, "Facility:", 100)
        {
        }

        public override void Load(XmlNode originalNode)
        {
            throw new NotImplementedException();
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            throw new NotImplementedException();
        }
    }
}
