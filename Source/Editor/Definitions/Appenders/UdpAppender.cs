// Copyright © 2018 Alex Leendertsen

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
            AddProperty(new RemoteAddress(Properties));
            AddProperty(new RemotePort(Properties));
            AddProperty(new LocalPort(Properties));
        }
    }
}
