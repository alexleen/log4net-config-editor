// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors;

namespace Editor.Interfaces
{
    internal interface IAppenderDefinition : IPropertyDefinition
    {
        AppenderDescriptor Descriptor { get; }
    }
}
