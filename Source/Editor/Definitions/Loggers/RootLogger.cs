// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Definitions.Base;
using Editor.Interfaces;
using Editor.Models.ConfigChildren;

namespace Editor.Definitions.Loggers
{
    internal class RootLogger : ElementDefinition
    {
        private readonly IElementConfiguration mAppenderConfiguration;

        public RootLogger(IElementConfiguration appenderConfiguration)
        {
            mAppenderConfiguration = appenderConfiguration;
        }

        public override string Name => "Root Logger";

        public override string Icon => "pack://application:,,,/Editor;component/Images/text-x-log.png";

        public override void Initialize()
        {
            AddProperty(new LevelProperty(Properties, true));
            AddProperty(new OutgoingRefs(Properties, mAppenderConfiguration, new AppenderFactory()));
        }
    }
}
