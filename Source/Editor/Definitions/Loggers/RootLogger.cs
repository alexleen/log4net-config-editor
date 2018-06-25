// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Definitions.Base;
using Editor.Interfaces;

namespace Editor.Definitions.Loggers
{
    internal class RootLogger : PropertyDefinition
    {
        private readonly IElementConfiguration mAppenderConfiguration;

        public RootLogger(IElementConfiguration appenderConfiguration)
        {
            mAppenderConfiguration = appenderConfiguration;
        }

        public override string Name => "Root Logger";

        public override void Initialize()
        {
            AddProperty(new LevelProperty(Properties));
            AddProperty(new OutgoingRefs(Properties, mAppenderConfiguration));
        }
    }
}
