// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    public class LevelProperty : LevelPropertyBase
    {
        private const string LevelName = "level";

        public LevelProperty(ReadOnlyCollection<IProperty> container, bool prependEmpty = false)
            : base(container, GridLength.Auto, "Level:", prependEmpty)
        {
        }

        public override void Load(XmlNode originalNode)
        {
            TryLoadSelectedLevel(originalNode.GetValueAttributeValueFromChildElement(LevelName));
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (!string.IsNullOrEmpty(SelectedLevel))
            {
                xmlDoc.CreateElementWithValueAttribute(LevelName, SelectedLevel).AppendTo(newNode);
            }
        }
    }
}
