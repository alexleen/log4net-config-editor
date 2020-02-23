// Copyright © 2020 Alex Leendertsen

using System.Collections.ObjectModel;
using Editor.ConfigProperties.Base;
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

        protected void AddProperty(IProperty property)
        {
            mProperties.Add(property);
        }

        protected void AddProperty(int index, IProperty property)
        {
            mProperties.Insert(index, property);

            //TODO PropertyBase should monitor for this change and raise prop changes
            foreach (IProperty prop in Properties)
            {
                ((PropertyBase)prop).OnPropertyChanged(nameof(IProperty.RowIndex));
                ((PropertyBase)prop).OnPropertyChanged(nameof(IProperty.RowHeight));
            }
        }

        protected void RemoveProperty(IProperty property)
        {
            mProperties.Remove(property);

            //TODO PropertyBase should monitor for this change and raise prop changes
            foreach (IProperty prop in Properties)
            {
                ((PropertyBase)prop).OnPropertyChanged(nameof(IProperty.RowIndex));
                ((PropertyBase)prop).OnPropertyChanged(nameof(IProperty.RowHeight));
            }
        }

        public abstract string Name { get; }

        public abstract string Icon { get; }

        public ReadOnlyObservableCollection<IProperty> Properties { get; }

        public abstract void Initialize();

        public IMessageBoxService MessageBoxService { get; set; }
    }
}
