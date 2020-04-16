// Copyright © 2020 Alex Leendertsen

using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    public class TypeAttribute : StringValueProperty
    {
        public TypeAttribute()
            : base("Type:", null)
        {
        }

        public TypeAttribute(AppenderDescriptor descriptor)
            : base("Type:", null)
        {
            //Will be overwritten when/if load is called
            Value = descriptor.TypeNamespace;
            IsReadOnly = true;
        }

        // public override void Load(XmlNode originalNode)
        // {
        //     SetValueIfNotNullOrEmpty(originalNode.Attributes[Log4NetXmlConstants.Type]?.Value);
        // }

        public override void Load(IElementConfiguration config)
        {
            if (config.Load(Log4NetXmlConstants.Type, out IValueResult result))
            {
                Value = result.AttributeValue;
            }
        }

        // public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        // {
        //     if (!string.IsNullOrEmpty(Value))
        //     {
        //         newNode.AppendAttribute(xmlDoc, TypeName, Value);
        //     }
        // }

        public override void Save(IElementConfiguration config)
        {
            if (!string.IsNullOrEmpty(Value))
            {
                config.Save(Log4NetXmlConstants.Type, Value);
            }
        }
    }
}
