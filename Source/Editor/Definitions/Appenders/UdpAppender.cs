// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.ConfigProperties.Base;
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
            AddProperty(new NumericProperty<ushort>("Remote Port:", "remotePort", 0));
            AddProperty(new NumericProperty<ushort>("Local Port:", "localPort", 0));
        }
    }
}
