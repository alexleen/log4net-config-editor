// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Windows;
using Editor.Windows.Appenders.Properties;

namespace Editor.ConfigProperties.Definitions
{
    internal class ConsoleAppender : AppenderSkeleton
    {
        public ConsoleAppender(IAppenderConfiguration appenderConfiguration, IMessageBoxService messageBoxService)
            : base(appenderConfiguration, messageBoxService)
        {
        }

        public override AppenderDescriptor Descriptor => AppenderDescriptor.Console;

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new Target(Properties));
        }
    }
}
