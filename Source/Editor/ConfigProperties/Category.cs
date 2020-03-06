// Copyright © 2020 Alex Leendertsen

using System;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    public class Category : StringValueProperty
    {
        public Category()
            : base("Category:", "category")
        {
        }

        public override void Load(XmlNode originalNode)
        {
            throw new NotImplementedException();
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (!string.IsNullOrWhiteSpace(Value))
            {
                //<category type="log4net.Layout.PatternLayout">
                //    <conversionPattern value="blah blah blah" />
                //</category>
                XmlNode layoutNode = xmlDoc.CreateElementWithAttribute("category", "type", LayoutDescriptor.Pattern.TypeNamespace);
                xmlDoc.CreateElementWithValueAttribute(Log4NetXmlConstants.ConversionPattern, Value).AppendTo(layoutNode);
                newNode.AppendChild(layoutNode);
            }
        }
    }
}
