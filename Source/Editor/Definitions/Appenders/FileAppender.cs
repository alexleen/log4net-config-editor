using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Windows;

namespace Editor.Definitions.Appenders
{
    internal class FileAppender : AppenderSkeleton
    {
        internal FileAppender(IAppenderConfiguration appenderConfiguration, IMessageBoxService messageBoxService)
            : base(appenderConfiguration, messageBoxService)
        {
        }

        public override AppenderDescriptor Descriptor => AppenderDescriptor.File;

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new File(Properties, mMessageBoxService));
            AddProperty(new LockingModel(Properties));
        }
    }
}
