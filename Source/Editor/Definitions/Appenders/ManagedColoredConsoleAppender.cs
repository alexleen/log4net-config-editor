// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Windows;

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
            AddProperty(new Mapping(Properties, MessageBoxService));
        }
    }
}
