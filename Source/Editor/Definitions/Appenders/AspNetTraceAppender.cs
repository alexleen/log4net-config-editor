// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Definitions.Appenders
{
    internal class AspNetTraceAppender : AppenderSkeleton
    {
        public AspNetTraceAppender(IElementConfiguration configuration)
            : base(configuration)
        {
        }

        public override string Name => "ASP.NET Trace Appender";

        public override AppenderDescriptor Descriptor => AppenderDescriptor.AspNetTrace;

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new Category());
        }
    }
}
