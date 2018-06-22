// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Windows;
using Editor.Windows.Appenders.Properties;

namespace Editor.ConfigProperties.Definitions
{
    internal class EventLogAppender : AppenderSkeleton
    {
        public EventLogAppender(IAppenderConfiguration appenderConfiguration, IMessageBoxService messageBoxService)
            : base(appenderConfiguration, messageBoxService)
        {
        }

        public override AppenderDescriptor Descriptor => AppenderDescriptor.EventLog;

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new LogName(Properties));
            AddProperty(new ApplicationName(Properties));
        }
    }
}
