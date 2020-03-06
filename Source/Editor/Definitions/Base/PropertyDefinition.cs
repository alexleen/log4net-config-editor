// Copyright © 2020 Alex Leendertsen

using System.Collections.ObjectModel;
using Editor.Interfaces;

namespace Editor.Definitions.Base
{
    internal abstract class ElementDefinition : IElementDefinition
    {
        private readonly ObservableCollection<IProperty> mProperties;

        protected ElementDefinition()
        {
            mProperties = new ObservableCollection<IProperty>();
            Properties = new ReadOnlyObservableCollection<IProperty>(mProperties);
        }

        public abstract string Name { get; }

        public abstract string Icon { get; }

        public ReadOnlyObservableCollection<IProperty> Properties { get; }

        public abstract void Initialize();

        public IMessageBoxService MessageBoxService { get; set; }

        protected void AddProperty(IProperty property)
        {
            mProperties.Add(property);
            property.RowIndex = mProperties.Count - 1;
        }

        protected void AddProperty(int index, IProperty property)
        {
            mProperties.Insert(index, property);
            UpdateIndices();
        }

        protected void RemoveProperty(IProperty property)
        {
            mProperties.Remove(property);
            UpdateIndices();
        }

        private void UpdateIndices()
        {
            for (int i = 0; i < Properties.Count; i++)
            {
                Properties[i].RowIndex = i;
            }
        }
    }
}
