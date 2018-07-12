// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Definitions.Base;

namespace Editor.Definitions.Renderer
{
    internal class RendererDefinition : ElementDefinition
    {
        public override string Name => "Renderer";

        public override string Icon => "pack://application:,,,/Editor;component/Images/list-add.png";

        public override void Initialize()
        {
            AddProperty(new RenderingClass(Properties));
            AddProperty(new RenderedClass(Properties));
        }
    }
}
