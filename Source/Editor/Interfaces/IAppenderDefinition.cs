// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors;

namespace Editor.Interfaces
{
    /// <summary>
    /// Definition of an appender.
    /// </summary>
    internal interface IAppenderDefinition : IElementDefinition
    {
        /// <summary>
        /// This appender's descriptor.
        /// </summary>
        AppenderDescriptor Descriptor { get; }
    }
}
