// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    public class LevelToMatch : LevelPropertyBase
    {
        private const string LevelMatchName = "levelToMatch";

        public LevelToMatch(ReadOnlyCollection<IProperty> container)
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
