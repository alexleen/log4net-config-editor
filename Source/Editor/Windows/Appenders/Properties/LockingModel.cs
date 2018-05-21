// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Descriptors;
using Editor.Utilities;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Appenders.Properties
{
    public class LockingModel : PropertyBase
    {
        private const string LockingModelName = "lockingModel";

        public LockingModel(ObservableCollection<IProperty> container)
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
