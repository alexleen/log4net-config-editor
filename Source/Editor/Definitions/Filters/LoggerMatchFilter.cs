using Editor.Descriptors;
using Editor.Windows.Filters.Properties;

namespace Editor.Definitions.Filters
{
    internal class LoggerMatchFilter : FilterSkeleton
    {
        public override FilterDescriptor Descriptor => FilterDescriptor.LoggerMatch;

        public override void Initialize()
        {
            AddProperty(new LoggerToMatch(Properties) { IsFocused = true });
            base.Initialize();
        }
    }
}
