// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;

namespace Editor.Definitions.Filters
{
    internal class LevelRangeFilter : FilterSkeleton
    {
        public override string Name => "Level Range Filter";

        public override FilterDescriptor Descriptor => FilterDescriptor.LevelRange;

        public override void Initialize()
        {
            AddProperty(new MinLevel(Properties));
            AddProperty(new MaxLevel(Properties));
            base.Initialize();
        }
    }
}
