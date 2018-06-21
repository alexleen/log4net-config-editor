using Editor.ConfigProperties.Definitions;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Windows;
using Editor.Windows.Appenders.Properties;

namespace Editor.Definitions.Appenders
{
    internal class ForwardingAppender : AppenderSkeleton
    {
        protected ForwardingAppender(IAppenderConfiguration appenderConfiguration, IMessageBoxService messageBoxService) : base(appenderConfiguration, messageBoxService)
        {
        }

        public override AppenderDescriptor Descriptor => AppenderDescriptor.Forwarding;

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new OutgoingRefs(Properties, mAppenderConfiguration));
        }

    }
}
