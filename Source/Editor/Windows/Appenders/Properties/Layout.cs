// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Descriptors;
using Editor.HistoryManager;
using Editor.Utilities;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Appenders.Properties
{
    public class Layout : PropertyBase
    {
        private readonly IHistoryManager mHistoryManager;
        private const string SimplePattern = "%level - %message%newline";
        private const string LayoutName = "layout";
        private const string ConversionPatternName = "conversionPattern";
        private string mOriginalPattern;

        public Layout(ObservableCollection<IProperty> container, IHistoryManager historyManager)
            : base(container, GridLength.Auto)
        {
            mHistoryManager = historyManager;

            Layouts = new[]
            {
                LayoutDescriptor.Simple,
                LayoutDescriptor.Pattern
            };

            HistoricalLayouts = mHistoryManager.Get();

            SelectedLayout = LayoutDescriptor.Simple;
        }

        public IEnumerable<LayoutDescriptor> Layouts { get; }

        public IEnumerable<string> HistoricalLayouts { get; }

        private LayoutDescriptor mSelectedLayout;

        public LayoutDescriptor SelectedLayout
        {
            get => mSelectedLayout;
            set
            {
                if (Equals(value, mSelectedLayout))
                {
                    return;
                }

                if (value == LayoutDescriptor.Simple || string.IsNullOrEmpty(mOriginalPattern))
                {
                    Pattern = SimplePattern;
                }
                else
                {
                    Pattern = mOriginalPattern;
                }

                mSelectedLayout = value;
                OnPropertyChanged();
            }
        }

        private string mPattern;

        public string Pattern
        {
            get => mPattern;
            set
            {
                if (value == mPattern)
                {
                    return;
                }

                mPattern = value;
                OnPropertyChanged();
            }
        }

        public override void Load(XmlNode originalNode)
        {
            if (LayoutDescriptor.TryFindByTypeNamespace(originalNode[LayoutName]?.Attributes["type"]?.Value, out LayoutDescriptor descriptor))
            {
                SelectedLayout = descriptor;

                string pattern = originalNode[LayoutName]?.GetValueAttributeValueFromChildElement(ConversionPatternName);

                if (!string.IsNullOrEmpty(pattern))
                {
                    Pattern = pattern;
                    mOriginalPattern = pattern;
                }
            }
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            if (string.IsNullOrEmpty(Pattern))
            {
                messageBoxService.ShowError("A pattern must be assigned to this appender.");
                return false;
            }

            return base.TryValidate(messageBoxService);
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            XmlNode layoutNode = xmlDoc.CreateElementWithAttribute(LayoutName, "type", SelectedLayout.TypeNamespace);

            if (SelectedLayout != LayoutDescriptor.Simple)
            {
                xmlDoc.CreateElementWithValueAttribute(ConversionPatternName, Pattern).AppendTo(layoutNode);
            }

            newNode.AppendChild(layoutNode);

            if (Pattern != mOriginalPattern)
            {
                mHistoryManager.Save(Pattern);
            }
        }
    }
}
