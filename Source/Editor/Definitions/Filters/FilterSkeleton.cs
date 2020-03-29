// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Definitions.Base;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Definitions.Filters
{
    internal abstract class FilterSkeleton : ElementDefinition, IFilterDefinition
    {
        public abstract FilterDescriptor Descriptor { get; }

        public override string Icon => "pack://application:,,,/Editor;component/Images/view-filter.png";

        public override void Initialize()
        {
            AddProperty(new BooleanPropertyBase("Accept on Match:", "acceptOnMatch", true));
        }
    }
}
