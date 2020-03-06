// Copyright © 2019 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Definitions.Appenders
{
    internal class UdpAppender : AppenderSkeleton
    {
        internal UdpAppender(IElementConfiguration configuration)
            : base(configuration)
        {
        }

        public override AppenderDescriptor Descriptor => AppenderDescriptor.Udp;

        public override string Name => "UDP Appender";

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new RemoteAddress());
            AddProperty(new Port("Remote Port:", "remotePort"));
            AddProperty(new Port("Local Port:", "localPort"));
        }
    }
}
