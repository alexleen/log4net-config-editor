// Copyright © 2020 Alex Leendertsen

using System;
using System.Linq;
using System.Windows;
using System.Xml;
using Editor.Utilities;

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

        public override void Load(XmlNode originalNode)
        {
            if (Enum.TryParse(originalNode.GetValueAttributeValueFromChildElement(mElementName), out TEnumType enumValue))
            {
                SelectedValue = enumValue.ToString();
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            xmlDoc.CreateElementWithValueAttribute(mElementName, SelectedValue).AppendTo(newNode);
        }
    }
}
