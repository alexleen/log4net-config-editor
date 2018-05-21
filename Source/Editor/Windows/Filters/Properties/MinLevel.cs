// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Filters.Properties
{
    public class MinLevel : LevelPropertyBase
    {
        private const string LevelMinName = "levelMin";

        public MinLevel(ObservableCollection<IProperty> container)
            : base(container, GridLength.Auto, "Min Level:", true)
        {
        }

        public override void Load(XmlNode originalNode)
        {
            TryLoadSelectedLevel(originalNode.GetValueAttributeValueFromChildElement(LevelMinName));
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (!string.IsNullOrEmpty(SelectedLevel))
            {
                xmlDoc.CreateElementWithValueAttribute(LevelMinName, SelectedLevel).AppendTo(newNode);
            }
        }
    }
}
