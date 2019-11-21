// Copyright © 2019 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.Definitions.Appenders
{
    internal abstract class BufferingAppenderSkeleton : AppenderSkeleton, IAppenderDefinition
    {
        protected BufferingAppenderSkeleton(IElementConfiguration configuration, bool requiresLayout)
            : base(configuration, requiresLayout)
        {
        }

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new BufferSize(Properties, 512));
            AddProperty(new Fix(Properties));
            AddProperty(new BooleanPropertyBase(Properties, "Lossy:", "lossy", false, false));
            AddProperty(new StringValueProperty(Properties, "Evaluator:", "evaluator"));
            AddProperty(new StringValueProperty(Properties, "Lossy Evaluator:", "lossyEvaluator"));
        }
    }
}
