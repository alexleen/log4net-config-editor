// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Utilities;

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
            AddProperty(new StringValueProperty("Quiet Writer:", "quietWriter", Log4NetXmlConstants.Type));
            AddProperty(new StringValueProperty("Writer:", "writer", Log4NetXmlConstants.Type));
        }
    }
}
