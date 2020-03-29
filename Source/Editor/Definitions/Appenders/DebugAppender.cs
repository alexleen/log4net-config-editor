// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Definitions.Appenders
{
    internal class DebugAppender : AppenderSkeleton
    {
        internal DebugAppender(IElementConfiguration configuration)
            : base(configuration)
        {
        }

        public override AppenderDescriptor Descriptor => AppenderDescriptor.Debug;

        public override string Name => "Debug Appender";

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new Category());
            AddProperty(new BooleanPropertyBase("Immediate Flush:", "immediateFlush", true));
        }
    }
}
