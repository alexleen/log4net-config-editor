// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.Definitions.Appenders
{
    internal class TextWriterAppender : AppenderSkeleton
    {
        public TextWriterAppender(IElementConfiguration configuration, bool requiresLayout = true)
            : base(configuration, requiresLayout)
        {
        }

        public override string Name => "Text Writer Appender";

        public override AppenderDescriptor Descriptor => AppenderDescriptor.TextWriter;

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new BooleanPropertyBase("Immediate Flush:", "immediateFlush", true));
            AddProperty(new StringValueProperty("Quiet Writer:", "quietWriter", Log4NetXmlConstants.Type) { ToolTip = "Sets the QuietTextWriter where logging events will be written to. QuietTextWriter does not throw exceptions when things go wrong. Instead, it delegates error handling to its IErrorHandler. (optional)" });
            AddProperty(new StringValueProperty("Writer:", "writer", Log4NetXmlConstants.Type) { ToolTip = "Sets the TextWriter where the log output will go. (optional)" });
        }
    }
}
