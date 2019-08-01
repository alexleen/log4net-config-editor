// Copyright © 2019 Alex Leendertsen

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Linq;
using SystemInterface.Xml;
using Editor.Enums;
using Editor.Interfaces;
using Editor.Models.Base;

namespace Editor.XML
{
    internal class SaveIndication : ConfigurationXml, IConfigurationXml
    {
        public SaveIndication(IMessageBoxService messageBoxService, ICanLoadAndSaveXml loadAndSave)
            : base(messageBoxService, loadAndSave)
        {
        }

        public IElementConfiguration CreateElementConfigurationFor(ModelBase originalModel, string newElementName)
        {
            return new ElementConfiguration(this, originalModel?.Node, mConfigXml.CreateElement(newElementName));
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

        private void ReevaluateSaveState()
        {
            IXmlDocument disk = mLoadAndSave.Load();

            if (XNode.DeepEquals(XDocument.Parse(disk.OuterXml), XDocument.Parse(mConfigXml.OuterXml)))
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
