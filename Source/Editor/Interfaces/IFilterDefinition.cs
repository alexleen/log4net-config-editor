// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors;

namespace Editor.ConfigProperties.Interfaces
{
    internal interface IFilterDefinition : IPropertyDefinition
    {
        FilterDescriptor Descriptor { get; }
    }
}
