// Copyright © 2020 Alex Leendertsen

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
            AddProperty(new NumericProperty<int>("Buffer Size:", "bufferSize", 512));
            AddProperty(new Fix());
            AddProperty(new BooleanPropertyBase("Lossy:", "lossy", false));
            AddProperty(new StringValueProperty("Evaluator:", "evaluator"));
            AddProperty(new StringValueProperty("Lossy Evaluator:", "lossyEvaluator"));
        }
    }
}
