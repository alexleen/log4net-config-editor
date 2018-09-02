// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;
using static log4net.Appender.LocalSyslogAppender;

namespace Editor.ConfigProperties
{
    internal class LocalSyslogFacility : EnumPropertyBase<SyslogFacility>
    {
        internal LocalSyslogFacility(ReadOnlyCollection<IProperty> container)
            : base(container, GridLength.Auto, "Facility:", 110, Log4NetXmlConstants.Facility)
        {
            SelectedValue = SyslogFacility.User.ToString();
        }
    }
}
