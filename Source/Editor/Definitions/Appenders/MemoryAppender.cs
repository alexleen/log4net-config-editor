// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Definitions.Appenders
{
    internal class MemoryAppender : AppenderSkeleton
    {
        public MemoryAppender(IElementConfiguration configuration)
            : base(configuration, false)
        {
        }

        public override string Name => "Memory Appender";

        public override AppenderDescriptor Descriptor => AppenderDescriptor.Memory;

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new Fix());
        }
    }
}
