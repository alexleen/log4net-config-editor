using System.Collections.ObjectModel;
using Editor.ConfigProperties.Interfaces;
using Editor.Windows.PropertyCommon;

namespace Editor.Definitions.Base
{
    internal abstract class PropertyDefinition : IPropertyDefinition
    {
        public abstract ReadOnlyCollection<IProperty> Properties { get; }

        public abstract void Initialize();
    }
}
