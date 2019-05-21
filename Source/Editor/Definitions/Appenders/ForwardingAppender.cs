// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Models.ConfigChildren;

namespace Editor.Definitions.Appenders
{
    internal class ForwardingAppender : AppenderSkeleton
    {
        internal ForwardingAppender(IElementConfiguration appenderConfiguration)
            : base(appenderConfiguration, false)
        {
        }

        public override string Name => "Forwarding Appender";

        public override AppenderDescriptor Descriptor => AppenderDescriptor.Forwarding;

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new OutgoingRefs(Properties, Configuration, new AppenderFactory()));
        }
    }
}
