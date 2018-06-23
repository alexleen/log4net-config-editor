// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Windows;

namespace Editor.Definitions.Appenders
{
    internal class AsyncAppender : ForwardingAppender
    {
        internal AsyncAppender(IAppenderConfiguration appenderConfiguration, IMessageBoxService messageBoxService)
            : base(appenderConfiguration, messageBoxService)
        {
        }

        public override AppenderDescriptor Descriptor => AppenderDescriptor.Async;

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new Fix(Properties) { SelectedPreset = Fix.PartialPreset });
            AddProperty(new BufferSize(Properties));
            base.AddAppenderSpecificProperties();
        }
    }
}
