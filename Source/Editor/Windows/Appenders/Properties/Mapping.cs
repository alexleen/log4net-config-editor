// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Models;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Appenders.Properties
{
    public class Mapping : PropertyBase
    {
        public Mapping(ObservableCollection<IProperty> container)
            : base(container, GridLength.Auto)
        {
        }

        public IEnumerable<MappingModel> Mappings { get; }

        public override void Load(XmlNode originalNode)
        {
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
        }
    }
}
