// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Windows;

namespace Editor.Definitions.Appenders
{
    internal class ForwardingAppender : AppenderSkeleton
    {
        internal ForwardingAppender(IAppenderConfiguration appenderConfiguration, IMessageBoxService messageBoxService)
            : base(appenderConfiguration, messageBoxService)
        {
        }

        public override AppenderDescriptor Descriptor => AppenderDescriptor.Forwarding;

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new OutgoingRefs(Properties, mAppenderConfiguration));
        }
    }
}
