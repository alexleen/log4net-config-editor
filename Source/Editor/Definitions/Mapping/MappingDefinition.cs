// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Definitions.Base;

namespace Editor.Definitions.Mapping
{
    internal class MappingDefinition : ElementDefinition
    {
        public override string Name => "Mapping";

        public override string Icon => "pack://application:,,,/Editor;component/Images/fill-color.png";

        public override void Initialize()
        {
            AddProperty(new LevelProperty(Properties));
            AddProperty(new ForeColor(Properties));
            AddProperty(new BackColor(Properties));
        }
    }
}
