// Copyright © 2020 Alex Leendertsen

using System;
using System.Linq;
using System.Windows;
using Editor.Interfaces;
using Editor.Utilities;
using Editor.XML;

namespace Editor.ConfigProperties.Base
{
    internal class EnumProperty<TEnumType> : MultiValuePropertyBase<string> where TEnumType : struct
    {
        private readonly string mElementName;

        public EnumProperty(string name, double width, string elementName)
            : base(GridLength.Auto, name, Enum.GetNames(typeof(TEnumType)), width)
        {
            mElementName = elementName;
            SelectedValue = Values.First();
        }

        public override void Load(IElementConfiguration config)
        {
            if (config.Load(Log4NetXmlConstants.Value, out IValueResult result, mElementName) && Enum.TryParse(result.AttributeValue, out TEnumType enumValue))
            {
                SelectedValue = enumValue.ToString();
            }
        }

        public override void Save(IElementConfiguration config)
        {
            config.Save(new Element(mElementName, new[] { (Log4NetXmlConstants.Value, SelectedValue) }));
        }
    }
}
