// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Definitions.Appenders
{
    internal class NetSendAppender : AppenderSkeleton
    {
        public NetSendAppender(IElementConfiguration configuration)
            : base(configuration)
        {
        }

        public override string Name => "Net Send Appender";

        public override AppenderDescriptor Descriptor => AppenderDescriptor.NetSend;

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new RequiredStringProperty("Recipient:", "recipient"));
            AddProperty(new RequiredStringProperty("Sender:", "sender"));
            AddProperty(new RequiredStringProperty("Server:", "server"));
        }
    }
}
