// Copyright © 2018 Alex Leendertsen

using Editor.Interfaces;
using System.Collections.ObjectModel;

namespace Editor.Definitions.Base
{
    internal abstract class PropertyDefinition : IPropertyDefinition
    {
        private readonly ObservableCollection<IProperty> mProperties;

        protected PropertyDefinition()
        {
            mProperties = new ObservableCollection<IProperty>();
            Properties = new ReadOnlyObservableCollection<IProperty>(mProperties);
        }

        protected void AddProperty(IProperty property)
        {
            mProperties.Add(property);
        }

        protected void AddProperty(int index, IProperty property)
        {
            mProperties.Insert(index, property);
        }

        protected void RemoveProperty(IProperty property)
        {
            mProperties.Remove(property);
        }

        public abstract string Name { get; }

        public ReadOnlyObservableCollection<IProperty> Properties { get; }

        public abstract void Initialize();

        public IMessageBoxService MessageBoxService { get; set; }
    }
}
