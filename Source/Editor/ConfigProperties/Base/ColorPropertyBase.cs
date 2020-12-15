// Copyright © 2020 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Editor.Interfaces;
using Editor.Utilities;
using Editor.XML;

namespace Editor.ConfigProperties.Base
{
    internal class ColorPropertyBase : PropertyBase
    {
        private readonly string mElementName;

        internal ColorPropertyBase(string name, string elementName)
            : base(GridLength.Auto)
        {
            Name = name;
            mElementName = elementName;
            Colors = Enum.GetValues(typeof(ConsoleColor)).Cast<ConsoleColor>();
        }

        public string Name { get; }

        public IEnumerable<ConsoleColor> Colors { get; }

        public ConsoleColor? SelectedColor { get; set; }

        public override void Load(IElementConfiguration config)
        {
            if (config.Load(Log4NetXmlConstants.Value, out IValueResult result, mElementName) && Enum.TryParse(result.AttributeValue, out ConsoleColor color))
            {
                SelectedColor = color;
            }
        }

        public override void Save(IElementConfiguration config)
        {
            if (SelectedColor != null)
            {
                config.Save(new Element(mElementName, new[] { (Log4NetXmlConstants.Value, SelectedColor.ToString()) }));
            }
        }
    }
}
