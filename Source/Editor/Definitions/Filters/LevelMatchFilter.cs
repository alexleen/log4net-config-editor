using Editor.ConfigProperties;
using Editor.Descriptors;

namespace Editor.Definitions.Filters
{
    class LevelMatchFilter : FilterSkeleton
    {
        public override FilterDescriptor Descriptor => FilterDescriptor.LevelMatch;

        public override void Initialize()
        {
            AddProperty(new LevelToMatch(Properties));
            base.Initialize();
        }
    }
}
