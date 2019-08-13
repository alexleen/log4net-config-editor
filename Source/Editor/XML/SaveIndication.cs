// Copyright © 2019 Alex Leendertsen

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Linq;
using SystemInterface.Xml;
using Editor.Enums;
using Editor.Interfaces;
using Editor.Models.Base;
using log4net.Core;

namespace Editor.XML
{
    internal class SaveIndication : ConfigurationXml, IConfigurationXml
    {
        public SaveIndication(IToastService toastService, ICanLoadAndSaveXml loadAndSave)
            : base(toastService, loadAndSave)
        {
        }

        public IElementConfiguration CreateElementConfigurationFor(ModelBase originalModel, string newElementName)
        {
            return new ElementConfiguration(this, originalModel?.Node, ConfigXml.CreateElement(newElementName));
        }

        public override bool? Reload()
        {
            bool? unrecognized = base.Reload();

            ReevaluateSaveState();

            return unrecognized;
        }

        public override async Task SaveAsync()
        {
            SaveState = SaveState.Saving;

            await base.SaveAsync();

            ReevaluateSaveState();
        }

        public override void RemoveChild(ModelBase child)
        {
            base.RemoveChild(child);

            ReevaluateSaveState();
        }

        private void ReevaluateSaveState()
        {
            IXmlDocument disk = LoadAndSave.Load();

            if (XNode.DeepEquals(XDocument.Parse(disk.OuterXml), XDocument.Parse(ConfigXml.OuterXml)))
            {
                SaveState = SaveState.Saved;
            }
            else
            {
                SaveState = SaveState.Changed;
            }
        }

        private SaveState mSaveState;

        public SaveState SaveState
        {
            get => mSaveState;
            private set
            {
                if (value == mSaveState)
                {
                    return;
                }

                mSaveState = value;
                OnPropertyChanged();
            }
        }

        public override bool Debug
        {
            get => base.Debug;
            set
            {
                base.Debug = value;
                ReevaluateSaveState();
            }
        }

        public override Update Update
        {
            get => base.Update;
            set
            {
                base.Update = value;
                ReevaluateSaveState();
            }
        }

        public override Level Threshold
        {
            get => base.Threshold;
            set
            {
                base.Threshold = value;
                ReevaluateSaveState();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
