// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.HistoryManager;
using Editor.Interfaces;

namespace Editor.Definitions.Appenders
{
    internal class FileAppender : AppenderSkeleton
    {
        internal FileAppender(IElementConfiguration configuration)
            : base(configuration, false)
        {
        }

        public override string Name => "File Appender";

        public override AppenderDescriptor Descriptor => AppenderDescriptor.File;

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new File(MessageBoxService, new HistoryManagerFactory(new SettingManager<string>())));
            AddProperty(new LockingModel());
        }
    }
}
