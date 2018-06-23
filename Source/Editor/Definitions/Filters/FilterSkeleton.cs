// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Definitions.Base;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Definitions.Filters
{
    internal abstract class FilterSkeleton : PropertyDefinition, IFilterDefinition
    {
        public abstract FilterDescriptor Descriptor { get; }

        public override void Initialize()
        {
            AddProperty(new AcceptOnMatch(Properties));
        }
    }
}
