using Editor.ConfigProperties;
using Editor.Descriptors;

namespace Editor.Definitions.Filters
{
    internal class PropertyMatchFilter : StringMatchFilter
    {
        public PropertyMatchFilter()
            : base(false)
        {
        }

        public override FilterDescriptor Descriptor => FilterDescriptor.Property;

        public override void Initialize()
        {
            AddProperty(new Key(Properties) { IsFocused = true });
            base.Initialize();
        }
    }
}
