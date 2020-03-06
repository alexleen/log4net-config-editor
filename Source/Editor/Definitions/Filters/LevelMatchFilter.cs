// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;

namespace Editor.Definitions.Filters
{
    internal class LevelMatchFilter : FilterSkeleton
    {
        public override string Name => "Level Match Filter";

        public override FilterDescriptor Descriptor => FilterDescriptor.LevelMatch;

        public override void Initialize()
        {
            AddProperty(new LevelToMatch());
            base.Initialize();
        }
    }
}
