// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    public class MaxLevel : LevelPropertyBase
    {
        private const string LevelMaxName = "levelMax";

        public MaxLevel(ReadOnlyCollection<IProperty> container)
            : base(container, GridLength.Auto, "Max Level:", true)
        {
        }

        public override void Load(XmlNode originalNode)
        {
            TryLoadSelectedLevel(originalNode.GetValueAttributeValueFromChildElement(LevelMaxName));
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (!string.IsNullOrEmpty(SelectedLevel))
            {
                xmlDoc.CreateElementWithValueAttribute(LevelMaxName, SelectedLevel).AppendTo(newNode);
            }
        }
    }
}
