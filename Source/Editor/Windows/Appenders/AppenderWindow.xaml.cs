// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Xml;
using Editor.Descriptors;
using Editor.Enums;
using Editor.Models;
using Editor.Windows.Filters;

namespace Editor.Windows.Appenders
{
    /// <summary>
    /// Interaction logic for AddFileAppenderWindow.xaml
    /// </summary>
    public abstract partial class AppenderWindow : Window
    {
        private readonly XmlDocument mConfigXml;
        private readonly XmlNode mLog4NetNode;
        private readonly XmlNode mAppenderNode;

        protected AppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode)
        {
            InitializeComponent();
            Owner = owner;
            mConfigXml = configXml;
            mLog4NetNode = log4NetNode;
            mAppenderNode = appenderNode;

            xLayoutComboBox.ItemsSource = new[]
            {
                LayoutDescriptor.Simple,
                LayoutDescriptor.Pattern
            };

            xFilterComboBox.ItemsSource = new[]
            {
                FilterDescriptor.DenyAll,
                FilterDescriptor.LevelMatch,
                FilterDescriptor.LevelRange,
                FilterDescriptor.LoggerMatch,
                FilterDescriptor.Mdc,
                FilterDescriptor.Ndc,
                FilterDescriptor.Property,
                FilterDescriptor.String
            };

            if (appenderNode != null)
            {
                Load();
            }
        }

        private void Load()
        {
            xNameTextBox.Text = mAppenderNode.Attributes?["name"]?.Value;

            LoadAppenderSpecificItems(mAppenderNode);

            string layoutType = mAppenderNode.SelectSingleNode("layout")?.Attributes?["type"]?.Value;
            if (LayoutDescriptor.TryFindByTypeNamespace(layoutType, out LayoutDescriptor descriptor))
            {
                xLayoutComboBox.SelectedItem = descriptor;
            }

            string conversionPattern = mAppenderNode.SelectSingleNode("//conversionPattern")?.Attributes?["value"]?.Value;
            xPatternTextBox.Text = conversionPattern;

            XmlNodeList filterNodes = mAppenderNode.SelectNodes("filter");

            ICollection<FilterModel> filters = new Collection<FilterModel>();

            if (filterNodes != null)
            {
                foreach (XmlNode filterNode in filterNodes)
                {
                    if (FilterDescriptor.TryFindByTypeNamespace(filterNode.Attributes?["type"]?.Value, out FilterDescriptor filterDescriptor))
                    {
                        filters.Add(new FilterModel(filterDescriptor, filterNode));
                    }
                }
            }

            xFilterDataGrid.ItemsSource = filters;
        }

        protected abstract void LoadAppenderSpecificItems(XmlNode appenderNode);

        private void HyperlinkOnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void SaveOnClick(object sender, RoutedEventArgs e)
        {
            if (!TryValidateInputs())
            {
                return;
            }

            if (mAppenderNode == null) //New appender
            {
                XmlAttribute nameAttribute = mConfigXml.CreateAttribute("name");
                nameAttribute.Value = xNameTextBox.Text;

                XmlAttribute appenderTypeAttribute = mConfigXml.CreateAttribute("type");
                appenderTypeAttribute.Value = AppenderDescriptor.File.TypeNamespace;

                XmlNode appenderNode = mConfigXml.CreateElement("appender");
                appenderNode.Attributes.Append(nameAttribute);
                appenderNode.Attributes.Append(appenderTypeAttribute);

                SaveAppenderSpecificItems(mConfigXml, appenderNode);

                XmlNode layoutNode = mConfigXml.CreateElement("layout");

                XmlAttribute layoutTypeAttribute = mConfigXml.CreateAttribute("type");
                layoutTypeAttribute.Value = ((LayoutDescriptor)xLayoutComboBox.SelectionBoxItem).TypeNamespace;
                layoutNode.Attributes.Append(layoutTypeAttribute);

                XmlAttribute convPatternValueAttr = mConfigXml.CreateAttribute("value");
                convPatternValueAttr.Value = xPatternTextBox.Text;

                XmlElement convPatternElement = mConfigXml.CreateElement("conversionPattern");
                convPatternElement.Attributes.Append(convPatternValueAttr);
                layoutNode.AppendChild(convPatternElement);

                appenderNode.AppendChild(layoutNode);

                mLog4NetNode.AppendChild(appenderNode);
            }

            Close();
        }

        protected virtual bool TryValidateInputs()
        {
            if (string.IsNullOrEmpty(xNameTextBox.Text))
            {
                MessageBox.Show(this, "A name must be assigned to this appender.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }            

            if (string.IsNullOrEmpty(xPatternTextBox.Text))
            {
                MessageBox.Show(this, "A pattern must be assigned to this appender.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        protected abstract void SaveAppenderSpecificItems(XmlDocument configXml, XmlNode appenderNode);

        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EditFilterOnClick(object sender, RoutedEventArgs e)
        {
            FilterModel filterModel = (FilterModel)((Button)sender).DataContext;
            ShowFilterWindow(filterModel);
            Load();
        }

        private void RemoveFilterOnClick(object sender, RoutedEventArgs e)
        {
            FilterModel filterModel = (FilterModel)((Button)sender).DataContext;
            mAppenderNode.RemoveChild(filterModel.Node);
            Load();
        }

        private void AddFilterOnClick(object sender, RoutedEventArgs e)
        {
            FilterDescriptor selectedFilter = (FilterDescriptor)xFilterComboBox.SelectionBoxItem;

            if (selectedFilter != FilterDescriptor.DenyAll)
            {
                ShowFilterWindow(new FilterModel(selectedFilter, null));
            }
            else
            {
                XmlAttribute filterTypeAttribute = mConfigXml.CreateAttribute("type");
                filterTypeAttribute.Value = FilterDescriptor.DenyAll.TypeNamespace;

                XmlElement filterElement = mConfigXml.CreateElement("filter");
                filterElement.Attributes.Append(filterTypeAttribute);

                mAppenderNode.AppendChild(filterElement);
            }

            Load();
        }

        private void ShowFilterWindow(FilterModel filterModel)
        {
            Window filterWindow = null;

            switch (filterModel.Descriptor.Type)
            {
                case FilterType.LevelMatch:
                    filterWindow = new LevelMatchFilterWindow(this, filterModel, mAppenderNode, mConfigXml);
                    break;
                case FilterType.LevelRange:
                    filterWindow = new LevelRangeFilterWindow(this, filterModel, mAppenderNode, mConfigXml);
                    break;
                case FilterType.LoggerMatch:
                    filterWindow = new LoggerMatchFilterWindow(this, filterModel, mAppenderNode, mConfigXml);
                    break;
                case FilterType.Mdc:
                    break;
                case FilterType.Ndc:
                    break;
                case FilterType.Property:
                    break;
                case FilterType.String:
                    filterWindow = new StringMatchFilterWindow(this, filterModel, mAppenderNode, mConfigXml);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            filterWindow?.ShowDialog();
        }

        private void InformationOnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this,
                            "Filters form a chain that the event has to pass through. Any filter along the way can accept the event and stop processing, deny the event and stop processing, or allow the event on to the next filter. If the event gets to the end of the filter chain without being denied it is implicitly accepted and will be logged. To reorder filters, click either '↑' or '↓'.",
                            "Information",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }
    }
}
