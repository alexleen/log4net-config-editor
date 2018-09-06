// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;

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

            AddProperty(new LocalSyslogFacility(Properties));
            AddProperty(new LocalIdentity(Properties));
        }
    }
}
