// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using Editor.Models;
using Editor.Utilities;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Appenders.Properties
{
    public class Mapping : PropertyBase
    {
        private readonly Window mOwner;

        public Mapping(Window owner, ObservableCollection<IProperty> container)
            : base(container, new GridLength(1, GridUnitType.Star))
        {
            mOwner = owner;
            Mappings = new ObservableCollection<MappingModel>();
            Add = new Command(ShowMappingWindowForAdd);
        }

        public ObservableCollection<MappingModel> Mappings { get; }

        public ICommand Add { get; }

        private void ShowMappingWindowForAdd()
        {
            MappingWindow mappingWindow = new MappingWindow(OnAdd, OnRemove) { Owner = mOwner };
            mappingWindow.ShowDialog();
        }

        private void OnAdd(MappingModel mappingModel)
        {
            Mappings.Add(mappingModel);
        }

        private void OnRemove(MappingModel mappingModel)
        {
            Mappings.Remove(mappingModel);
        }

        public override void Load(XmlNode originalNode)
        {
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            //Either foreground or background must be selected
            return base.TryValidate(messageBoxService);
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
        }
    }
}
