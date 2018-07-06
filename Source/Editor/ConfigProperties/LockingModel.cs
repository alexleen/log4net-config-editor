// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    public class LockingModel : PropertyBase
    {
        private const string LockingModelName = "lockingModel";

        public LockingModel(ReadOnlyCollection<IProperty> container)
            : base(container, GridLength.Auto)
        {
            LockingModels = new[] { LockingModelDescriptor.Exclusive, LockingModelDescriptor.Minimal, LockingModelDescriptor.InterProcess };
            SelectedModel = LockingModelDescriptor.Exclusive;
        }

        public IEnumerable<LockingModelDescriptor> LockingModels { get; }

        public LockingModelDescriptor SelectedModel { get; set; }

        public override void Load(XmlNode originalNode)
        {
            string modelType = originalNode[LockingModelName]?.Attributes["type"]?.Value;
            if (LockingModelDescriptor.TryFindByTypeNamespace(modelType, out LockingModelDescriptor descriptor))
            {
                SelectedModel = descriptor;
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            //Exclusive is the default and does not need to be specified in the XML if chosen
            if (SelectedModel != LockingModelDescriptor.Exclusive)
            {
                xmlDoc.CreateElementWithAttribute(LockingModelName, "type", SelectedModel.TypeNamespace).AppendTo(newNode);
            }
        }
    }
}
