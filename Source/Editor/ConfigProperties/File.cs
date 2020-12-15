// Copyright © 2020 Alex Leendertsen

using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Editor.ConfigProperties.Base;
using Editor.HistoryManager;
using Editor.Interfaces;
using Editor.Utilities;
using Editor.XML;

namespace Editor.ConfigProperties
{
    public class File : PropertyBase
    {
        private const string FileName = "file";
        private const string AppendToFileName = "appendToFile";
        private const string TypeName = "type";
        private const string PatternStringTypeName = "log4net.Util.PatternString";
        private readonly IHistoryManager mHistoryManager;
        private readonly IMessageBoxService mMessageBoxService;

        private string mFilePath;

        private bool mOverwrite;

        private bool mPatternString;

        public File(IMessageBoxService messageBoxService, IHistoryManagerFactory historyManagerFactory)
            : base(GridLength.Auto)
        {
            Open = new Command(OpenFile);
            mMessageBoxService = messageBoxService;
            mHistoryManager = historyManagerFactory.CreateFilePathHistoryManager();
            HistoricalFiles = mHistoryManager.Get();
        }

        public ICommand Open { get; }

        public IEnumerable<string> HistoricalFiles { get; }

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

        public bool PatternString
        {
            get => mPatternString;
            set
            {
                if (value == mPatternString)
                {
                    return;
                }

                mPatternString = value;
                OnPropertyChanged();
            }
        }

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

            if (showDialog.IsTrue())
            {
                FilePath = filePath;
            }
        }

        public override void Load(IElementConfiguration config)
        {
            if (config.Load(Log4NetXmlConstants.Value, out IValueResult pathResult, FileName) && !string.IsNullOrEmpty(pathResult.AttributeValue))
            {
                FilePath = pathResult.AttributeValue;
            }

            PatternString = config.Load(TypeName, out IValueResult patternResult, FileName) && patternResult.AttributeValue == PatternStringTypeName;

            if (config.Load(Log4NetXmlConstants.Value, out IValueResult appendResult, AppendToFileName) && !string.IsNullOrEmpty(appendResult.AttributeValue) && bool.TryParse(appendResult.AttributeValue, out bool append))
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

        public override void Save(IElementConfiguration config)
        {
            IList<(string attrName, string attrValue)> attrs = new List<(string attrName, string attrValue)>();

            if (PatternString)
            {
                attrs.Add((TypeName, PatternStringTypeName));
            }

            attrs.Add((Log4NetXmlConstants.Value, FilePath));
            config.Save(new Element(FileName, attrs));

            //"appendToFile" is true by default, so we only need to change it to false if Overwrite is true
            if (Overwrite)
            {
                config.Save(new Element(AppendToFileName, new[] { (Log4NetXmlConstants.Value, "false") }));
            }

            mHistoryManager.Save(FilePath);
        }
    }
}
