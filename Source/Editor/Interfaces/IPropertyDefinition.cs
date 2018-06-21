// Copyright © 2018 Alex Leendertsen

using Editor.Windows.PropertyCommon;
using System.Collections.ObjectModel;

namespace Editor.ConfigProperties.Interfaces
{
    public interface IPropertyDefinition
    {
        void Initialize();

        ReadOnlyCollection<IProperty> Properties { get; }
    }
}
