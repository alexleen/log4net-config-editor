using Editor.ConfigProperties;
using Editor.Definitions.Base;

namespace Editor.Definitions.Mapping
{
    class MappingDefinition : PropertyDefinition
    {
        public override string Name => "Mapping";

        public override void Initialize()
        {
            AddProperty(new LevelProperty(Properties));
            AddProperty(new ForeColor(Properties));
            AddProperty(new BackColor(Properties));
        }
    }
}
