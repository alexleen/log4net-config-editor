using Editor.ConfigProperties;
using Editor.Descriptors;

namespace Editor.Definitions.Filters
{
    class LevelRangeFilter : FilterSkeleton
    {
        public override FilterDescriptor Descriptor => FilterDescriptor.LevelRange;

        public override void Initialize()
        {
            AddProperty(new MinLevel(Properties));
            AddProperty(new MaxLevel(Properties));
            base.Initialize();
        }
    }
}
