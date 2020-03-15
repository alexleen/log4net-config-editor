// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Definitions.Appenders
{
    internal class BufferingForwardingAppender : BufferingAppenderSkeleton
    {
        public BufferingForwardingAppender(IElementConfiguration configuration)
            : base(configuration, false)
        {
        }

        public override string Name => "Buffering Forwarding Appender";

        public override AppenderDescriptor Descriptor => AppenderDescriptor.BufferingForwarding;

        protected override void AddAppenderSpecificProperties()
        {
            base.AddAppenderSpecificProperties();
            AddProperty(new OutgoingRefs(Configuration));
        }
    }
}
