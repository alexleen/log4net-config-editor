// Copyright © 2019 Alex Leendertsen

using Editor.ConfigProperties;
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
            AddProperty(new BufferSize(Properties));
            AddProperty(new Fix(Properties));
            AddProperty(new Lossy(Properties));
            //Evaluator (& LossyEvaluator?)
        }
    }
}
