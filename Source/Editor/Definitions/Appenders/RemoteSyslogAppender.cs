// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Utilities;
using static log4net.Appender.RemoteSyslogAppender;

namespace Editor.Definitions.Appenders
{
    internal class RemoteSyslogAppender : UdpAppender
    {
        internal RemoteSyslogAppender(IElementConfiguration configuration)
            : base(configuration)
        {
        }

        public override AppenderDescriptor Descriptor => AppenderDescriptor.RemoteSyslog;

        public override string Name => "Remote Syslog Appender";

        protected override void AddAppenderSpecificProperties()
        {
            base.AddAppenderSpecificProperties();

            AddProperty(new EnumProperty<SyslogFacility>("Facility:", 110, Log4NetXmlConstants.Facility) { SelectedValue = SyslogFacility.User.ToString() });
            AddProperty(new RemoteIdentity());
        }
    }
}
