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
            Add = new Command(ShowMappingWindow);
        }

        public IEnumerable<MappingModel> Mappings { get; }

        public ICommand Add { get; }

        private void ShowMappingWindow()
        {
            MappingWindow mappingWindow = new MappingWindow { Owner = mOwner };
            mappingWindow.ShowDialog();
        }

        public override void Load(XmlNode originalNode)
        {
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
        }
    }
}
