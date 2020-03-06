// Copyright © 2020 Alex Leendertsen

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.HistoryManager;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    public class Layout : PropertyBase
    {
        private const string SimplePattern = "%level - %message%newline";
        private const string LayoutName = "layout";
        private const string ConversionPatternName = "conversionPattern";
        private readonly IHistoryManager mHistoryManager;
        private string mOriginalPattern;

        private string mPattern;

        private LayoutDescriptor mSelectedLayout;

        public Layout(IHistoryManagerFactory historyManagerFactory, bool required = true)
            : base(GridLength.Auto)
        {
            mHistoryManager = historyManagerFactory.CreatePatternsHistoryManager();

            if (required)
            {
                Layouts = new[]
                {
                    LayoutDescriptor.Simple,
                    LayoutDescriptor.Pattern
                };

                SelectedLayout = LayoutDescriptor.Simple;
            }
            else
            {
                Layouts = new[]
                {
                    LayoutDescriptor.None,
                    LayoutDescriptor.Simple,
                    LayoutDescriptor.Pattern
                };

                SelectedLayout = LayoutDescriptor.None;
            }

            HistoricalLayouts = mHistoryManager.Get();
        }

        public IEnumerable<LayoutDescriptor> Layouts { get; }

        public IEnumerable<string> HistoricalLayouts { get; }

        public LayoutDescriptor SelectedLayout
        {
            get => mSelectedLayout;
            set
            {
                if (Equals(value, mSelectedLayout))
                {
                    return;
                }

                if (value == LayoutDescriptor.None)
                {
                    Pattern = string.Empty;
                }
                else if (!string.IsNullOrEmpty(mOriginalPattern))
                {
                    Pattern = mOriginalPattern;
                }
                else if (value == LayoutDescriptor.Pattern && HistoricalLayouts.Any())
                {
                    Pattern = HistoricalLayouts.First();
                }
                else
                {
                    Pattern = SimplePattern;
                }

                mSelectedLayout = value;
                OnPropertyChanged();
            }
        }

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
            if (string.IsNullOrEmpty(Pattern) && SelectedLayout != LayoutDescriptor.None)
            {
                messageBoxService.ShowError("A pattern must be assigned to this appender.");
                return false;
            }

            return base.TryValidate(messageBoxService);
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (SelectedLayout == LayoutDescriptor.None)
            {
                return;
            }

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
