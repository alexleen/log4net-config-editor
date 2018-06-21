using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Windows;
using Editor.Windows.Appenders.Properties;

namespace Editor.Definitions.Appenders
{
    internal class AsyncAppender : ForwardingAppender
    {
        protected AsyncAppender(IAppenderConfiguration appenderConfiguration, IMessageBoxService messageBoxService)
            : base(appenderConfiguration, messageBoxService)
        {
        }

        public override AppenderDescriptor Descriptor => AppenderDescriptor.Async;

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new Fix(Properties) { SelectedPreset = Fix.PartialPreset });
            AddProperty(new BufferSize(Properties));
            base.AddAppenderSpecificProperties();
        }
    }
}
