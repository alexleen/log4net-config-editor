// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    internal class MaxLevel : LevelPropertyBase
    {
        private const string LevelMaxName = "levelMax";

        public MaxLevel()
            : base(GridLength.Auto, "Max Level:", true)
        {
        }

        public override void Load(XmlNode originalNode)
        {
            TryLoadSelectedLevel(originalNode.GetValueAttributeValueFromChildElement(LevelMaxName));
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (!string.IsNullOrEmpty(SelectedValue))
            {
                xmlDoc.CreateElementWithValueAttribute(LevelMaxName, SelectedValue).AppendTo(newNode);
            }
        }
    }
}
