// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors;

namespace Editor.Interfaces
{
    /// <summary>
    /// Definition of a filter.
    /// </summary>
    internal interface IFilterDefinition : IElementDefinition
    {
        /// <summary>
        /// This filter's descriptor.
        /// </summary>
        FilterDescriptor Descriptor { get; }
    }
}
