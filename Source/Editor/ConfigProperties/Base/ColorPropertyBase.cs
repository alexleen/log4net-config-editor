// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml;
using Editor.Utilities;

namespace Editor.ConfigProperties.Base
{
    public abstract class ColorPropertyBase : PropertyBase
    {
        private readonly string mElementName;

        protected ColorPropertyBase(GridLength rowHeight, string name, string elementName)
            : base(rowHeight)
        {
            Name = name;
            mElementName = elementName;
            Colors = Enum.GetValues(typeof(ConsoleColor)).Cast<ConsoleColor>();
        }

        public string Name { get; }

        public IEnumerable<ConsoleColor> Colors { get; }

        public ConsoleColor? SelectedColor { get; set; }

        public override void Load(XmlNode originalNode)
        {
            string value = originalNode.GetValueAttributeValueFromChildElement(mElementName);

            if (Enum.TryParse(value, out ConsoleColor color))
            {
                SelectedColor = color;
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (SelectedColor != null)
            {
                xmlDoc.CreateElementWithValueAttribute(mElementName, SelectedColor.ToString()).AppendTo(newNode);
            }
        }
    }
}
