using Editor.Descriptors;
using Editor.Windows.Filters.Properties;

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
