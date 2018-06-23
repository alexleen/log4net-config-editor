using System.Collections.ObjectModel;
using Editor.Interfaces;

namespace Editor.Definitions.Base
{
    internal abstract class PropertyDefinition : IPropertyDefinition
    {
        public abstract ReadOnlyCollection<IProperty> Properties { get; }

        public abstract void Initialize();
    }
}
