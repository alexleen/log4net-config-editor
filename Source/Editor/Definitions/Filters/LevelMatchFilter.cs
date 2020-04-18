// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Descriptors;

namespace Editor.Definitions.Filters
{
    internal class LevelMatchFilter : FilterSkeleton
    {
        public override string Name => "Level Match Filter";

        public override FilterDescriptor Descriptor => FilterDescriptor.LevelMatch;

        public override void Initialize()
        {
            AddProperty(new LevelPropertyBase("Level to Match:", "levelToMatch"));
            base.Initialize();
        }
    }
}
