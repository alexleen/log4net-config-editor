// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Definitions.Appenders
{
    internal class TextWriterAppender : AppenderSkeleton
    {
        public TextWriterAppender(IElementConfiguration configuration)
            : base(configuration)
        {
        }

        public override string Name => "Text Writer Appender";

        public override AppenderDescriptor Descriptor => AppenderDescriptor.TextWriter;

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new BooleanPropertyBase("Immediate Flush:", "immediateFlush", true));
            AddProperty(new RequiredStringProperty("Quiet Writer:", "quietWriter"));
            AddProperty(new RequiredStringProperty("Writer:", "writer"));
        }
    }
}
