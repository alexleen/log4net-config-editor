// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.ConfigProperties.Base;
using Editor.Definitions.Base;
using Editor.Interfaces;

namespace Editor.Definitions.Loggers
{
    internal class Logger : ElementDefinition
    {
        private readonly IElementConfiguration mConfiguration;

        public Logger(IElementConfiguration configuration)
        {
            mConfiguration = configuration;
        }

        public override string Name => "Logger";

        public override string Icon => "pack://application:,,,/Editor;component/Images/text-x-log.png";

        public override void Initialize()
        {
            AddProperty(new Name(mConfiguration));
            AddProperty(new Aditivity());
            AddProperty(new LevelPropertyBase("Level:", "level", true));
            AddProperty(new OutgoingRefs(mConfiguration));
        }
    }
}
