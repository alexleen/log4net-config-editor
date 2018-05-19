// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using Editor.Utilities;
using Microsoft.Win32;

namespace Editor.Windows.Appenders.Properties
{
    public class File : AppenderPropertyBase
    {
        private const string FileName = "file";
        private const string AppendToFileName = "appendToFile";

        public File(ObservableCollection<IAppenderProperty> container)
            : base(container, GridLength.Auto)
        {
            Open = new Command(OpenFile);
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
            OpenFileDialog ofd = new OpenFileDialog();

            bool? showDialog = ofd.ShowDialog();

            if (showDialog.HasValue && showDialog.Value)
            {
                FilePath = ofd.FileName;
            }
        }

        public override void Load(XmlNode originalAppenderNode)
        {
            string file = originalAppenderNode.GetValueAttributeValueFromChildElement(FileName);
            if (!string.IsNullOrEmpty(file))
            {
                FilePath = file;
            }

            string appendToFile = originalAppenderNode.GetValueAttributeValueFromChildElement(AppendToFileName);
            if (!string.IsNullOrEmpty(appendToFile))
            {
                Overwrite = appendToFile == "false";
            }
        }

        public override bool TryValidate()
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                MessageBox.Show("A file must be assigned to this appender.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return base.TryValidate();
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newAppenderNode)
        {
            xmlDoc.CreateElementWithValueAttribute(FileName, FilePath).AppendTo(newAppenderNode);

            //"appendToFile" is true by default, so we only need to change it to false if Overwrite is true
            if (Overwrite)
            {
                xmlDoc.CreateElementWithValueAttribute(AppendToFileName, "false").AppendTo(newAppenderNode);
            }
        }
    }
}
