// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Definitions.Base;
using Editor.Interfaces;

namespace Editor.Definitions.Loggers
{
    internal class RootLogger : ElementDefinition
    {
        private readonly IElementConfiguration mConfiguration;

        public RootLogger(IElementConfiguration Configuration)
        {
            mConfiguration = Configuration;
        }

        public override string Name => "Root Logger";

        public override string Icon => "pack://application:,,,/Editor;component/Images/text-x-log.png";

        public override void Initialize()
        {
            AddProperty(new LevelProperty(Properties, true));
            AddProperty(new OutgoingRefs(Properties, mConfiguration));
        }
    }
}
