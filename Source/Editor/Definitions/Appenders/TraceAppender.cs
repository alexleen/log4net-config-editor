// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Definitions.Appenders
{
    internal class TraceAppender : AppenderSkeleton
    {
        internal TraceAppender(IElementConfiguration configuration)
            : base(configuration)
        {
        }

        public override AppenderDescriptor Descriptor => AppenderDescriptor.Trace;

        public override string Name => "Trace Appender";

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new Category());
            AddProperty(new BooleanPropertyBase("Immediate Flush:", "immediateFlush", true));
        }
    }
}
