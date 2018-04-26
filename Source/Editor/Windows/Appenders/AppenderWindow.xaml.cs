// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Xml;
using Editor.Descriptors;
using Editor.Enums;
using Editor.Models;
using Editor.Utilities;
using Editor.Windows.Filters;

namespace Editor.Windows.Appenders
{
    /// <summary>
    /// Interaction logic for AddFileAppenderWindow.xaml
    /// </summary>
    public abstract partial class AppenderWindow : Window
    {
        public static readonly GridLength AutoGridRowHeight = GridLength.Auto;

        private readonly XmlDocument mConfigXml;
        private readonly XmlNode mLog4NetNode;

        /// <summary>
        /// Appender node this window is referencing.
        /// Never null. Could be an existing appender for editing or it may be "empty" -
        /// i.e. a new new appender element with no attributes or children.
        /// </summary>
        private readonly XmlNode mAppenderNode;

        private readonly ChangeType mChangeType;

        public ObservableCollection<FilterModel> Filters { get; }

        protected AppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode, ChangeType changeType)
        {
            InitializeComponent();

            Owner = owner ?? throw new ArgumentException(nameof(owner));
            mConfigXml = configXml ?? throw new ArgumentException(nameof(configXml));
            mLog4NetNode = log4NetNode ?? throw new ArgumentException(nameof(log4NetNode));
            mAppenderNode = appenderNode ?? throw new ArgumentException(nameof(appenderNode));
            mChangeType = changeType;
            Filters = new ObservableCollection<FilterModel>();

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
                FilterDescriptor.String
            };

            xNameTextBox.Focus();

            Load();
        }

        private void Load()
        {
            ReplaceIfNotNullOrEmpty(mAppenderNode.Attributes?["name"]?.Value, xNameTextBox);

            LoadAppenderSpecificItems(mAppenderNode);

            string layoutType = mAppenderNode["layout"]?.Attributes["type"]?.Value;
            if (LayoutDescriptor.TryFindByTypeNamespace(layoutType, out LayoutDescriptor descriptor))
            {
                xLayoutComboBox.SelectedItem = descriptor;
            }

            LoadConversionPattern();

            XmlNodeList filterNodes = mAppenderNode.SelectNodes("filter");

            if (filterNodes != null)
            {
                foreach (XmlNode filterNode in filterNodes)
                {
                    if (FilterDescriptor.TryFindByTypeNamespace(filterNode.Attributes?["type"]?.Value, out FilterDescriptor filterDescriptor))
                    {
                        Filters.Add(new FilterModel(filterDescriptor, filterNode));
                    }
                }
            }

            xFilterDataGrid.ItemsSource = Filters;

            ICollection<LoggerModel> appenderRefModels = new List<LoggerModel>();

            XmlNodeList asyncAppenders = mLog4NetNode.SelectNodes("appender[@type='Log4Net.Async.AsyncForwardingAppender,Log4Net.Async']");

            if (asyncAppenders != null)
            {
                foreach (XmlNode asyncAppender in asyncAppenders)
                {
                    XmlNode appenderRef = asyncAppender.SelectSingleNode($"appender-ref[@ref='{xNameTextBox.Text}']");
                    string name = asyncAppender.Attributes?["name"]?.Value;
                    appenderRefModels.Add(new LoggerModel(name, asyncAppender, appenderRef != null));
                }
            }

            XmlNode root = mLog4NetNode.SelectSingleNode("root");

            if (root != null)
            {
                XmlNode appenderRef = root.SelectSingleNode($"appender-ref[@ref='{xNameTextBox.Text}']");
                appenderRefModels.Add(new LoggerModel("root", root, appenderRef != null));
            }

            xRefsListBox.ItemsSource = appenderRefModels;
        }

        private void LoadConversionPattern()
        {
            ReplaceIfNotNullOrEmpty(mAppenderNode["layout"]?["conversionPattern"]?.Attributes["value"]?.Value, xPatternTextBox);
        }

        /// <summary>
        /// Sets the destination string to the source string iff source string it not null or empty.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        protected static void ReplaceIfNotNullOrEmpty(string source, TextBox destination)
        {
            if (!string.IsNullOrEmpty(source))
            {
                destination.Text = source;
            }
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

            //If we're editing, just replace the existing appender with a new one
            XmlNode appenderNode = mChangeType == ChangeType.Add ? mAppenderNode : mConfigXml.CreateElement("appender");
            appenderNode.AppendAttribute(mConfigXml, "name", xNameTextBox.Text);
            appenderNode.AppendAttribute(mConfigXml, "type", Descriptor.TypeNamespace);

            SaveAppenderSpecificItems(mConfigXml, appenderNode);

            SaveLayout(appenderNode);

            foreach (FilterModel filter in Filters)
            {
                appenderNode.AppendChild(filter.Node);
            }

            foreach (LoggerModel loggerModel in xRefsListBox.ItemsSource)
            {
                XmlNodeList appenderRefs = loggerModel.LoggerNode.SelectNodes($"appender-ref[@ref='{xNameTextBox.Text}']");

                if (loggerModel.IsEnabled)
                {
                    if (appenderRefs == null || appenderRefs.Count == 0)
                    {
                        //Doesn't exist - add
                        mConfigXml.CreateElementWithAttribute("appender-ref", "ref", xNameTextBox.Text).AppendTo(loggerModel.LoggerNode);
                    }
                    else if (appenderRefs.Count == 1)
                    {
                        //Only one - we're good
                    }
                    else
                    {
                        //More than one - remove all
                        foreach (XmlNode appenderRef in appenderRefs)
                        {
                            loggerModel.LoggerNode.RemoveChild(appenderRef);
                        }

                        //Add
                        mConfigXml.CreateElementWithAttribute("appender-ref", "ref", xNameTextBox.Text).AppendTo(loggerModel.LoggerNode);
                    }
                }
                else
                {
                    if (appenderRefs != null && appenderRefs.Count > 0)
                    {
                        foreach (XmlNode appenderRef in appenderRefs)
                        {
                            loggerModel.LoggerNode.RemoveChild(appenderRef);
                        }
                    }
                }
            }

            if (mChangeType == ChangeType.Add)
            {
                mLog4NetNode.AppendChild(appenderNode);
            }
            else
            {
                mLog4NetNode.ReplaceChild(appenderNode, mAppenderNode);
            }

            Close();
        }

        protected virtual void SaveLayout(XmlNode appenderNode)
        {
            XmlNode layoutNode = mConfigXml.CreateElementWithAttribute("layout", "type", ((LayoutDescriptor)xLayoutComboBox.SelectionBoxItem).TypeNamespace);

            if ((LayoutDescriptor)xLayoutComboBox.SelectedItem != LayoutDescriptor.Simple)
            {
                mConfigXml.CreateElementWithAttribute("conversionPattern", "value", xPatternTextBox.Text).AppendTo(layoutNode);
            }

            appenderNode.AppendChild(layoutNode);
        }

        protected abstract AppenderDescriptor Descriptor { get; }

        protected virtual bool TryValidateInputs()
        {
            //TODO name uniqueness
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
        }

        private void RemoveFilterOnClick(object sender, RoutedEventArgs e)
        {
            FilterModel filterModel = (FilterModel)((Button)sender).DataContext;
            Filters.Remove(filterModel);
        }

        private void AddFilterOnClick(object sender, RoutedEventArgs e)
        {
            FilterDescriptor selectedFilter = (FilterDescriptor)xFilterComboBox.SelectionBoxItem;
            FilterModel filterModel;

            if (selectedFilter != FilterDescriptor.DenyAll)
            {
                filterModel = new FilterModel(selectedFilter, null);
                ShowFilterWindow(filterModel);
            }
            else
            {
                XmlElement filterElement = mConfigXml.CreateElementWithAttribute("filter", "type", FilterDescriptor.DenyAll.TypeNamespace);
                filterModel = new FilterModel(FilterDescriptor.DenyAll, filterElement);
            }

            Filters.Add(filterModel);
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

        private void HelpOnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this,
                            "Filters form a chain that the event has to pass through. Any filter along the way can accept the event and stop processing, deny the event and stop processing, or allow the event on to the next filter. If the event gets to the end of the filter chain without being denied it is implicitly accepted and will be logged. To reorder filters, click either '↑' or '↓'.",
                            "Help",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        private void LayoutComboBoxOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LayoutDescriptor layoutDescriptor = (LayoutDescriptor)((ComboBox)sender).SelectedItem;

            switch (layoutDescriptor.Type)
            {
                case LayoutType.Simple:
                    xPatternTextBox.Text = "%level - %message%newline";
                    xPatternTextBox.IsEnabled = false;
                    break;
                case LayoutType.Pattern:
                    xPatternTextBox.IsEnabled = true;
                    LoadConversionPattern();
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(layoutDescriptor.Type), (int)layoutDescriptor.Type, typeof(LayoutType));
            }
        }

        private void MoveFilterUpOnClick(object sender, RoutedEventArgs e)
        {
            FilterModel filterModel = (FilterModel)((Button)sender).DataContext;
            int oldIndex = Filters.IndexOf(filterModel);
            int newIndex = oldIndex == 0 ? 0 : oldIndex - 1;
            Filters.Move(oldIndex, newIndex);
        }

        private void MoveFilterDownOnClick(object sender, RoutedEventArgs e)
        {
            FilterModel filterModel = (FilterModel)((Button)sender).DataContext;
            int oldIndex = Filters.IndexOf(filterModel);
            int newIndex = oldIndex == Filters.Count - 1 ? Filters.Count - 1 : oldIndex + 1;
            Filters.Move(oldIndex, newIndex);
        }
    }
}
