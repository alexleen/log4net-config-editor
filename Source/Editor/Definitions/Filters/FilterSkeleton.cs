using Editor.Definitions.Base;
using Editor.Descriptors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Editor.ConfigProperties;
using Editor.Interfaces;

namespace Editor.Definitions.Filters
{
    internal abstract class FilterSkeleton : PropertyDefinition, IFilterDefinition
    {
        private readonly IList<IProperty> mProperties;

        public FilterSkeleton()
        {
            mProperties = new List<IProperty>();
            Properties = new ReadOnlyCollection<IProperty>(mProperties);
        }

        public abstract FilterDescriptor Descriptor { get; }

        public override ReadOnlyCollection<IProperty> Properties { get; }

        public override void Initialize()
        {
            AddProperty(new AcceptOnMatch(Properties));
        }

        protected void AddProperty(IProperty property)
        {
            mProperties.Add(property);
        }
    }
}
