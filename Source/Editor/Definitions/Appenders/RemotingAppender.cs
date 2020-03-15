// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Definitions.Appenders
{
    internal class RemotingAppender : BufferingAppenderSkeleton
    {
        public RemotingAppender(IElementConfiguration configuration)
            : base(configuration, false)
        {
        }

        public override string Name => "Remoting Appender";

        public override AppenderDescriptor Descriptor => AppenderDescriptor.Remoting;

        protected override void AddAppenderSpecificProperties()
        {
            base.AddAppenderSpecificProperties();
            AddProperty(new RequiredStringProperty("Sink:", "sink"));
        }
    }
}
