// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Definitions.Appenders
{
    internal class FileAppender : AppenderSkeleton
    {
        internal FileAppender(IElementConfiguration configuration)
            : base(configuration)
        {
        }

        public override string Name => "File Appender";

        public override AppenderDescriptor Descriptor => AppenderDescriptor.File;

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new File(Properties, MessageBoxService));
            AddProperty(new LockingModel(Properties));
        }
    }
}
