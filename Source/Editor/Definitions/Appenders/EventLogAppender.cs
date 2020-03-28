// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Utilities;

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
            AddProperty(new RequiredStringProperty("Log Name:", "logName"));
            AddProperty(new RequiredStringProperty("Application Name:", "applicationName"));
            AddProperty(new StringValueProperty("Security Context:", "securityContext", Log4NetXmlConstants.Type));
        }
    }
}
