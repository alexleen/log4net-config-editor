// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Definitions.Appenders
{
    internal class ForwardingAppender : AppenderSkeleton
    {
        internal ForwardingAppender(IElementConfiguration appenderConfiguration)
            : base(appenderConfiguration)
        {
        }

        public override string Name => "Forwarding Appender";

        public override AppenderDescriptor Descriptor => AppenderDescriptor.Forwarding;

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new OutgoingRefs(Properties, Configuration));
        }
    }
}
