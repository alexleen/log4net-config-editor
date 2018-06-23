// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Windows;

namespace Editor.Definitions.Appenders
{
    internal class ConsoleAppender : AppenderSkeleton
    {
        public ConsoleAppender(IAppenderConfiguration appenderConfiguration, IMessageBoxService messageBoxService)
            : base(appenderConfiguration, messageBoxService)
        {
        }
        
        public override string Name => "Console Appender";

        public override AppenderDescriptor Descriptor => AppenderDescriptor.Console;

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new Target(Properties));
        }
    }
}
