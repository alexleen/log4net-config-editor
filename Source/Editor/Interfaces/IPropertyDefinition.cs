// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;

namespace Editor.Interfaces
{
    public interface IPropertyDefinition
    {
        string Name { get; }
        
        void Initialize();

        ReadOnlyObservableCollection<IProperty> Properties { get; }
    }
}
