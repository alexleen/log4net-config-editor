// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Definitions.Appenders
{
    internal class EventLogAppender : AppenderSkeleton
    {
        public EventLogAppender(IElementConfiguration appenderConfiguration)
            : base(appenderConfiguration)
        {
        }

        public override string Name => "Event Log Appender";

        public override AppenderDescriptor Descriptor => AppenderDescriptor.EventLog;

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new LogName(Properties));
            AddProperty(new ApplicationName(Properties));
        }
    }
}
