// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors;

namespace Editor.ConfigProperties.Interfaces
{
    public interface IAppenderDefinition : IPropertyDefinition
    {
        AppenderDescriptor Descriptor { get; }
    }
}
