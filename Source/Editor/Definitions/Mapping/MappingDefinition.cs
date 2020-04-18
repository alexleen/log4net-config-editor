// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Definitions.Base;

namespace Editor.Definitions.Mapping
{
    internal class MappingDefinition : ElementDefinition
    {
        public override string Name => "Mapping";

        public override string Icon => "pack://application:,,,/Editor;component/Images/fill-color.png";

        public override void Initialize()
        {
            AddProperty(new LevelPropertyBase("Level:", "level"));
            AddProperty(new ColorPropertyBase("Foreground:", "foreColor"));
            AddProperty(new ColorPropertyBase("Background:", "backColor"));
        }
    }
}
