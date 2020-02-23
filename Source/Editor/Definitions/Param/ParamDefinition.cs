// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Definitions.Base;
using Editor.Interfaces;

namespace Editor.Definitions.Param
{
    internal class ParamDefinition : ElementDefinition
    {
        private readonly IElementConfiguration mConfiguration;

        public ParamDefinition(IElementConfiguration configuration)
        {
            mConfiguration = configuration;
        }

        public override string Name => "Param";

        public override string Icon => "pack://application:,,,/Editor;component/Images/run-build-configure.png";

        public override void Initialize()
        {
            AddProperty(new Name(mConfiguration) { IsFocused = true });
            AddProperty(new Value());
            AddProperty(new TypeAttribute());
        }
    }
}
