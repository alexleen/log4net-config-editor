// Copyright © 2020 Alex Leendertsen

using System.Collections.Generic;
using System.Windows;
using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    public class LockingModel : PropertyBase
    {
        private const string LockingModelName = "lockingModel";

        public LockingModel()
            : base(GridLength.Auto)
        {
            LockingModels = new[] { LockingModelDescriptor.Exclusive, LockingModelDescriptor.Minimal, LockingModelDescriptor.InterProcess };
            SelectedModel = LockingModelDescriptor.Exclusive;
        }

        public IEnumerable<LockingModelDescriptor> LockingModels { get; }

        public LockingModelDescriptor SelectedModel { get; set; }

        public override void Load(IElementConfiguration config)
        {
            string modelType = config.OriginalNode[LockingModelName]?.Attributes["type"]?.Value;
            if (LockingModelDescriptor.TryFindByTypeNamespace(modelType, out LockingModelDescriptor descriptor))
            {
                SelectedModel = descriptor;
            }
        }

        public override void Save(IElementConfiguration config)
        {
            //Exclusive is the default and does not need to be specified in the XML if chosen
            if (SelectedModel != LockingModelDescriptor.Exclusive)
            {
                config.Save((LockingModelName, "type", SelectedModel.TypeNamespace));
            }
        }
    }
}
