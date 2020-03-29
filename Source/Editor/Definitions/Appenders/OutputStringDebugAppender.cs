// Copyright © 2020 Alex Leendertsen

using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Definitions.Appenders
{
    internal class OutputStringDebugAppender : AppenderSkeleton
    {
        public OutputStringDebugAppender(IElementConfiguration configuration)
            : base(configuration)
        {
        }

        public override string Name => "Output Debug String Appender";

        public override AppenderDescriptor Descriptor => AppenderDescriptor.OutputDebugString;
    }
}
