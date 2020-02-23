// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Utilities;
using static log4net.Appender.LocalSyslogAppender;

namespace Editor.Definitions.Appenders
{
    internal class LocalSyslogAppender : AppenderSkeleton
    {
        internal LocalSyslogAppender(IElementConfiguration configuration)
            : base(configuration)
        {
        }

        public override AppenderDescriptor Descriptor => AppenderDescriptor.LocalSyslog;

        public override string Name => "Local Syslog Appender";

        protected override void AddAppenderSpecificProperties()
        {
            base.AddAppenderSpecificProperties();

            AddProperty(new EnumProperty<SyslogFacility>("Facility:", 110, Log4NetXmlConstants.Facility) { SelectedValue = SyslogFacility.User.ToString() });
            AddProperty(new StringValueProperty("Identity:", Log4NetXmlConstants.Identity));
        }
    }
}
