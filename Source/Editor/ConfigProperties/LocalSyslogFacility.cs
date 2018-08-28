using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using static log4net.Appender.LocalSyslogAppender;

namespace Editor.ConfigProperties
{
    internal class LocalSyslogFacility : EnumPropertyBase<SyslogFacility>
    {
        protected LocalSyslogFacility(ReadOnlyCollection<IProperty> container, GridLength rowHeight) 
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
