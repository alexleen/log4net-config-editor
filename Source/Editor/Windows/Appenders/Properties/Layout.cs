// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Descriptors;
using Editor.Utilities;

namespace Editor.Windows.Appenders.Properties
{
    public class Layout : AppenderPropertyBase
    {
        private const string SimplePattern = "%level - %message%newline";
        private const string LayoutName = "layout";
        private const string ConversionPatternName = "conversionPattern";
        private string mOriginalPattern;

        public Layout(ObservableCollection<IAppenderProperty> container)
            : base(container, GridLength.Auto)
        {
            Layouts = new[]
            {
                LayoutDescriptor.Simple,
                LayoutDescriptor.Pattern
            };

            SelectedLayout = LayoutDescriptor.Simple;
        }

        public IEnumerable<LayoutDescriptor> Layouts { get; }

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

        public override void Load(XmlNode originalAppenderNode)
        {
            string layoutType = originalAppenderNode[LayoutName]?.Attributes["type"]?.Value;
            if (LayoutDescriptor.TryFindByTypeNamespace(layoutType, out LayoutDescriptor descriptor))
            {
                SelectedLayout = descriptor;
            }

            string pattern = originalAppenderNode[LayoutName]?.GetValueAttributeValueFromChildElement(ConversionPatternName);

            if (!string.IsNullOrEmpty(pattern))
            {
                Pattern = pattern;
                mOriginalPattern = pattern;
            }
        }

        public override bool TryValidate()
        {
            if (string.IsNullOrEmpty(Pattern))
            {
                MessageBox.Show("A pattern must be assigned to this appender.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return base.TryValidate();
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newAppenderNode)
        {
            XmlNode layoutNode = xmlDoc.CreateElementWithAttribute(LayoutName, "type", SelectedLayout.TypeNamespace);

            if (SelectedLayout != LayoutDescriptor.Simple)
            {
                xmlDoc.CreateElementWithValueAttribute(ConversionPatternName, Pattern).AppendTo(layoutNode);
            }

            newAppenderNode.AppendChild(layoutNode);
        }
    }
}
