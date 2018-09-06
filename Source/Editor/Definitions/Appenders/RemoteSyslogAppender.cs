// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;

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

            AddProperty(new RemoteSyslogFacility(Properties));
            AddProperty(new RemoteIdentity(Properties));
        }
    }
}
