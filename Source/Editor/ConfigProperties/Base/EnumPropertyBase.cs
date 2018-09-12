// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties.Base
{
    internal abstract class EnumPropertyBase<TEnumType> : MultiValuePropertyBase<string> where TEnumType : struct
    {
        private readonly string mElementName;

        protected EnumPropertyBase(ReadOnlyCollection<IProperty> container, GridLength rowHeight, string name, double width, string elementName)
            : base(container, rowHeight, name, Enum.GetNames(typeof(TEnumType)), width)
        {
            mElementName = elementName;
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
