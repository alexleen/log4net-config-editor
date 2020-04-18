// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Descriptors;

namespace Editor.Definitions.Filters
{
    internal class LevelRangeFilter : FilterSkeleton
    {
        public override string Name => "Level Range Filter";

        public override FilterDescriptor Descriptor => FilterDescriptor.LevelRange;

        public override void Initialize()
        {
            AddProperty(new LevelPropertyBase("Min Level:", "levelMin", true));
            AddProperty(new LevelPropertyBase("Max Level:", "levelMax", true));
            base.Initialize();
        }
    }
}
