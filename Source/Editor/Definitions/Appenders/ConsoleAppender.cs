// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Definitions.Appenders
{
    internal class ConsoleAppender : AppenderSkeleton
    {
        public ConsoleAppender(IElementConfiguration appenderConfiguration)
            : base(appenderConfiguration)
        {
        }

        public override string Name => "Console Appender";

        public override AppenderDescriptor Descriptor => AppenderDescriptor.Console;

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new Target());
        }
    }
}
