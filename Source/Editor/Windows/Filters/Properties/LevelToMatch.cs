// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Filters.Properties
{
    public class LevelToMatch : LevelPropertyBase
    {
        private const string LevelMatchName = "levelToMatch";

        public LevelToMatch(ObservableCollection<IProperty> container)
            : base(container, GridLength.Auto, "Level to Match:")
        {
        }

        public override void Load(XmlNode originalNode)
        {
            TryLoadSelectedLevel(originalNode.GetValueAttributeValueFromChildElement(LevelMatchName));
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            xmlDoc.CreateElementWithValueAttribute(LevelMatchName, SelectedLevel).AppendTo(newNode);
        }
    }
}
