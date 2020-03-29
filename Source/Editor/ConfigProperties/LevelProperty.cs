// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    internal class LevelProperty : LevelPropertyBase
    {
        private const string LevelName = "level";

        public LevelProperty(bool prependEmpty = false)
            : base(GridLength.Auto, "Level:", prependEmpty)
        {
        }

        public override void Load(XmlNode originalNode)
        {
            TryLoadSelectedLevel(originalNode.GetValueAttributeValueFromChildElement(LevelName));
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (!string.IsNullOrEmpty(SelectedValue))
            {
                xmlDoc.CreateElementWithValueAttribute(LevelName, SelectedValue).AppendTo(newNode);
            }
        }
    }
}
