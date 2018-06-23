// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;

namespace Editor.Interfaces
{
    public interface IPropertyDefinition
    {
        void Initialize();

        ReadOnlyCollection<IProperty> Properties { get; }
    }
}
