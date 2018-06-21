using Editor.ConfigProperties.Definitions;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Windows;
using Editor.Windows.Appenders.Properties;

namespace Editor.Definitions.Appenders
{
    internal class ManagedColoredConsoleAppender : ConsoleAppender
    {
        protected ManagedColoredConsoleAppender(IAppenderConfiguration appenderConfiguration, IMessageBoxService messageBoxService)
            : base(appenderConfiguration, messageBoxService)
        {
        }

        public override AppenderDescriptor Descriptor => AppenderDescriptor.ManagedColor;

        protected override void AddAppenderSpecificProperties()
        {
            base.AddAppenderSpecificProperties();
            AddProperty(new Mapping(this, Properties));
        }
    }
}
