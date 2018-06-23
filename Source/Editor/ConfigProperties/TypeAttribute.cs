// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    public class TypeAttribute : StringValueProperty
    {
        private readonly AppenderDescriptor mDescriptor;
        private const string TypeName = "type";

        public TypeAttribute(ReadOnlyCollection<IProperty> container, AppenderDescriptor descriptor)
            : base(container, GridLength.Auto, "Type:")
        {
            mDescriptor = descriptor;
            IsReadOnly = true;
        }

        public override void Load(XmlNode originalNode)
        {
            string value = originalNode.Attributes?[TypeName]?.Value;
            Value = !string.IsNullOrEmpty(value) ? value : mDescriptor.TypeNamespace;
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            newNode.AppendAttribute(xmlDoc, TypeName, Value);
        }
    }
}
