// Copyright © 2019 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    public class TypeAttribute : MultiValuePropertyBase<AppenderDescriptor>
    {
        private const string TypeName = "type";

        private static readonly AppenderDescriptor[] sValues =
        {
            AppenderDescriptor.Async
        };

        public TypeAttribute(ReadOnlyCollection<IProperty> container)
            : base(container, GridLength.Auto, "Type:", sValues, 100)
        {
        }

        public TypeAttribute(ReadOnlyCollection<IProperty> container, AppenderDescriptor descriptor)
            : base(container, GridLength.Auto, "Type:", sValues, 100)
        {
            //Will be overwritten when/if load is called (?)
            SelectedValue = descriptor;
        }

        public override void Load(XmlNode originalNode)
        {
            //SetValueIfNotNullOrEmpty(originalNode.Attributes[TypeName]?.Value);
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
//            if (!string.IsNullOrEmpty(Value))
//            {
//                newNode.AppendAttribute(xmlDoc, TypeName, Value);
//            }
        }
    }
}
