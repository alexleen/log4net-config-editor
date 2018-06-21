// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using Editor.Descriptors;
using Editor.Windows.PropertyCommon;

namespace Editor.ConfigProperties.Interfaces
{
    public interface IPropertyDefinition
    {
        AppenderDescriptor Descriptor { get; }

        void Initialize();
        
        ReadOnlyCollection<IProperty> Properties { get; }
    }
}
