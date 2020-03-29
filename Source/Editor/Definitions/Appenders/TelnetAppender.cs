// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Definitions.Appenders
{
    internal class TelnetAppender : AppenderSkeleton
    {
        internal TelnetAppender(IElementConfiguration configuration)
            : base(configuration)
        {
        }

        public override AppenderDescriptor Descriptor => AppenderDescriptor.Telnet;

        public override string Name => "Telnet Appender";

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new NumericProperty<ushort>("Port:", "port", 23));
        }
    }
}
