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
        public Layout(ObservableCollection<IAppenderProperty> container, XmlNode appenderNode)
            : base(container, GridLength.Auto, appenderNode)
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

                if (value == LayoutDescriptor.Simple)
                {
                    Pattern = "%level - %message%newline";
                }
                else
                {
                    LoadPattern();
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

        public override void Load()
        {
            string layoutType = AppenderNode["layout"]?.Attributes["type"]?.Value;
            if (LayoutDescriptor.TryFindByTypeNamespace(layoutType, out LayoutDescriptor descriptor))
            {
                SelectedLayout = descriptor;
            }

            LoadPattern();
        }

        private void LoadPattern()
        {
            string pattern = AppenderNode["layout"]?["conversionPattern"]?.Attributes["value"]?.Value;

            if (!string.IsNullOrEmpty(pattern))
            {
                Pattern = pattern;
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

        public override void Save(XmlDocument xmlDoc)
        {
            XmlNode layoutNode = xmlDoc.CreateElementWithAttribute("layout", "type", SelectedLayout.TypeNamespace);

            if (SelectedLayout != LayoutDescriptor.Simple)
            {
                xmlDoc.CreateElementWithAttribute("conversionPattern", "value", Pattern).AppendTo(layoutNode);
            }

            AppenderNode.AppendChild(layoutNode);
        }
    }
}
