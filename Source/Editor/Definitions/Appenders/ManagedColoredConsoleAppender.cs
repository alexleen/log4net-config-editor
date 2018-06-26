// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Definitions.Appenders
{
    internal class ManagedColoredConsoleAppender : ConsoleAppender
    {
        internal ManagedColoredConsoleAppender(IElementConfiguration appenderConfiguration)
            : base(appenderConfiguration)
        {
        }

        public override string Name => "Managed Color Console Appender";

        public override AppenderDescriptor Descriptor => AppenderDescriptor.ManagedColor;

        protected override void AddAppenderSpecificProperties()
        {
            base.AddAppenderSpecificProperties();
            AddProperty(new ConfigProperties.Mapping(Properties, MessageBoxService));
        }
    }
}
