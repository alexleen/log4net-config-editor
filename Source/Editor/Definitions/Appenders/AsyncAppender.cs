// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Definitions.Appenders
{
    internal class AsyncAppender : ForwardingAppender
    {
        internal AsyncAppender(IElementConfiguration appenderConfiguration)
            : base(appenderConfiguration)
        {
        }

        public override string Name => "Async Appender";

        public override AppenderDescriptor Descriptor => AppenderDescriptor.Async;

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new Fix { SelectedPreset = Fix.PartialPreset });
            AddProperty(new BufferSize(1000));
            base.AddAppenderSpecificProperties();
        }
    }
}
