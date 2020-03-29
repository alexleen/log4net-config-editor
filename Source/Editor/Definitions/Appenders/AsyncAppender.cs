// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.ConfigProperties.Base;
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
            AddProperty(new NumericProperty<int>("Buffer Size:", "bufferSize", 1000));
            base.AddAppenderSpecificProperties();
        }
    }
}
