// Copyright © 2018 Alex Leendertsen

using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    public class TypeAttribute : StringValueProperty
    {
        private const string TypeName = "type";

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

        public override void Load(XmlNode originalNode)
        {
            SetValueIfNotNullOrEmpty(originalNode.Attributes[TypeName]?.Value);
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (!string.IsNullOrEmpty(Value))
            {
                newNode.AppendAttribute(xmlDoc, TypeName, Value);
            }
        }
    }
}
