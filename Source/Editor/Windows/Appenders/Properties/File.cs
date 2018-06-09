// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.PropertyCommon;
using Microsoft.Win32;

namespace Editor.Windows.Appenders.Properties
{
    public class File : PropertyBase
    {
        private const string FileName = "file";
        private const string AppendToFileName = "appendToFile";
        private readonly IMessageBoxService mMessageBoxService;

        public File(ObservableCollection<IProperty> container, IMessageBoxService messageBoxService)
            : base(container, GridLength.Auto)
        {
            Open = new Command(OpenFile);
            mMessageBoxService = messageBoxService;
        }

        public ICommand Open { get; }

        private string mFilePath;

        public string FilePath
        {
            get => mFilePath;
            set
            {
                if (value == mFilePath)
                {
                    return;
                }

                mFilePath = value;
                OnPropertyChanged();
            }
        }

        private bool mOverwrite;

        public bool Overwrite
        {
            get => mOverwrite;
            set
            {
                if (value == mOverwrite)
                {
                    return;
                }

                mOverwrite = value;
                OnPropertyChanged();
            }
        }

        private void OpenFile()
        {
            bool? showDialog = mMessageBoxService.ShowOpenFileDialog(out string filePath);

            if (showDialog.HasValue && showDialog.Value)
            {
                FilePath = filePath;
            }
        }

        public override void Load(XmlNode originalNode)
        {
            string file = originalNode.GetValueAttributeValueFromChildElement(FileName);
            if (!string.IsNullOrEmpty(file))
            {
                FilePath = file;
            }

            string appendToFile = originalNode.GetValueAttributeValueFromChildElement(AppendToFileName);
            if (!string.IsNullOrEmpty(appendToFile) && bool.TryParse(appendToFile, out bool append))
            {
                Overwrite = !append;
            }
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                messageBoxService.ShowError("A file must be assigned to this appender.");
                return false;
            }

            return base.TryValidate(messageBoxService);
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            xmlDoc.CreateElementWithValueAttribute(FileName, FilePath).AppendTo(newNode);

            //"appendToFile" is true by default, so we only need to change it to false if Overwrite is true
            if (Overwrite)
            {
                xmlDoc.CreateElementWithValueAttribute(AppendToFileName, "false").AppendTo(newNode);
            }
        }
    }
}
