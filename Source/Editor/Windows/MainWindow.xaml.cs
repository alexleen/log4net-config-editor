// Copyright © 2018 Alex Leendertsen

using Editor.Definitions.Factory;
using Editor.Descriptors;
using Editor.Descriptors.Base;
using Editor.HistoryManager;
using Editor.Interfaces;
using Editor.Models;
using Editor.Utilities;
using Editor.Windows.SizeLocation;
using Editor.XML;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Editor.Windows
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IMessageBoxService
    {
        private const string UpdateMerge = "Merge";
        private const string UpdateOverwrite = "Overwrite";
        private readonly HistoryManager.HistoryManager mConfigHistoryManager;
        private IConfigurationXml mConfigurationXml;

        public MainWindow()
            : base("MainWindowPlacement")
        {
            InitializeComponent();

            mConfigHistoryManager = new HistoryManager.HistoryManager("HistoricalConfigs", new SettingManager<string>());

            xAddAppenderButton.ItemsSource = new[]
            {
                AppenderDescriptor.Console,
                AppenderDescriptor.File,
                AppenderDescriptor.RollingFile,
                AppenderDescriptor.EventLog,
                AppenderDescriptor.Async,
                AppenderDescriptor.Forwarding,
                AppenderDescriptor.ManagedColor
            };

            xUpdateComboBox.ItemsSource = new[] { UpdateMerge, UpdateOverwrite };

            xThresholdComboBox.ItemsSource = Log4NetUtilities.LevelsByName.Keys;

            Title = $"log4net Configuration Editor - v{Assembly.GetEntryAssembly().GetName().Version.ToString(3)}";
        }

        private void MainWindowOnLoaded(object sender, RoutedEventArgs e)
        {
            IEnumerable<string> configs = mConfigHistoryManager.Get();

            if (configs.Any())
            {
                string config = configs.First();
                RefreshConfigComboBox(config);
                LoadFromFile(config);
            }
        }

        private void ConfigComboBoxOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedConfig = (string)xConfigComboBox.SelectedItem;
            RefreshConfigComboBox(selectedConfig);
            LoadFromFile(selectedConfig);
        }

        private void OpenThereOnClick(object sender, RoutedEventArgs e)
        {
            string selectedConfig = (string)xConfigComboBox.SelectedItem;

            if (!string.IsNullOrEmpty(selectedConfig))
            {
                Process.Start(selectedConfig);
            }
        }

        private void OpenHereOnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Config Files (*.xml, *.config) | *.xml; *.config" };

            bool? showDialog = ofd.ShowDialog(this);

            if (showDialog.Value)
            {
                RefreshConfigComboBox(ofd.FileName);
                LoadFromFile(ofd.FileName);
            }
        }

        /// <summary>
        /// Save the specified file name to the set of historical configs.
        /// Sets the config ComboBox's ItemsSource to the set of historical configs.
        /// Sets the config ComboBox's to the specified filename.
        /// </summary>
        /// <param name="fileName"></param>
        private void RefreshConfigComboBox(string fileName)
        {
            mConfigHistoryManager.Save(fileName);
            xConfigComboBox.ItemsSource = mConfigHistoryManager.Get();
            xConfigComboBox.SelectedItem = fileName;
        }

        private void ReloadOnClick(object sender, RoutedEventArgs e)
        {
            ReloadFromFile();
        }

        private void SaveOnClick(object sender, RoutedEventArgs e)
        {
            SaveToFile();
        }

        private void SandAndCloseOnClick(object sender, RoutedEventArgs e)
        {
            SaveToFile();
            Close();
        }

        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveToFile()
        {
            mConfigurationXml.Save();
        }

        private void ReloadFromFile()
        {
            LoadFromFile((string)xConfigComboBox.SelectedItem);
        }

        private void LoadFromRam()
        {
            mConfigurationXml.Reload();
        }

        private void LoadFromFile(string filename)
        {
            mConfigurationXml = new ConfigurationXml(filename, this);
            mConfigurationXml.Load();
        }

        private void AddAppenderItemOnClick(object appender)
        {
            OpenElementWindow((AppenderDescriptor)appender, null);
        }

        private void EditAppenderOnClick(object sender, RoutedEventArgs e)
        {
            object dataContext = ((DataGridRow)sender).DataContext;

            if (dataContext is AppenderModel appenderModel)
            {
                OpenElementWindow(appenderModel.Descriptor, appenderModel);
            }
            else if (dataContext is RendererModel rendererModel)
            {
                OpenElementWindow(RendererDescriptor.Renderer, rendererModel);
            }
            else if (dataContext is LoggerModel loggerModel)
            {
                OpenElementWindow(LoggerDescriptor.Logger, loggerModel);
            }
            else if (dataContext is ChildModel childModel)
            {
                OpenElementWindow(LoggerDescriptor.Root, childModel);
            }
        }

        private void RemoveAppenderOnClick(object sender, RoutedEventArgs e)
        {
            foreach (ChildModel childModel in xChildren.SelectedItems)
            {
                mConfigurationXml.Children.Remove(childModel);

                if (childModel is AppenderModel appenderModel)
                {
                    mConfigurationXml.RemoveRefsTo(appenderModel);
                }
            }

            LoadFromRam();
        }

        private void RemoveRefsOnClick(object sender, RoutedEventArgs e)
        {
            foreach (ChildModel childModel in xChildren.SelectedItems)
            {
                if (childModel is AppenderModel appenderModel)
                {
                    mConfigurationXml.RemoveRefsTo(appenderModel);
                }
            }

            LoadFromRam();
        }

        private void AddRefsButtonOnItemClick(object obj)
        {
            ChildModel destination = (ChildModel)obj;

            foreach (AppenderModel appenderModel in xChildren.SelectedItems.OfType<AppenderModel>())
            {
                XmlUtilities.AddAppenderRefToNode(mConfigurationXml.ConfigXml, destination.Node, appenderModel.Name);
            }

            LoadFromRam();
        }

        private void OpenElementWindow(DescriptorBase descriptor, ChildModel model)
        {
            IElementConfiguration configuration = mConfigurationXml.CreateElementConfigurationFor(model, descriptor);

            ElementWindow elementWindow = new ElementWindow(configuration,
                                                            DefinitionFactory.Create(descriptor, configuration),
                                                            WindowSizeLocationFactory.Create(descriptor),
                                                            new AppenderSaveStrategy(configuration))
            { Owner = this };

            elementWindow.ShowDialog();

            LoadFromRam();
        }

        private void AddRootClick(object sender, RoutedEventArgs e)
        {
            if (xChildren.ItemsSource.Cast<ChildModel>().Any(cm => cm.ElementName == "root"))
            {
                MessageBox.Show(this, "This configuration already contains a root logger.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            OpenElementWindow(LoggerDescriptor.Root, null);
        }

        private void AddRendererClick(object sender, RoutedEventArgs e)
        {
            OpenElementWindow(RendererDescriptor.Renderer, null);
        }

        private void AddLoggerClick(object sender, RoutedEventArgs e)
        {
            OpenElementWindow(LoggerDescriptor.Logger, null);
        }

        #region IMessageBoxService

        public void ShowError(string message)
        {
            MessageBox.Show(this, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowWarning(string message)
        {
            MessageBox.Show(this, message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void ShowInformation(string message)
        {
            MessageBox.Show(this, message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowWindow(Window window)
        {
            window.Owner = this;
            window.ShowDialog();
        }

        public bool? ShowOpenFileDialog(out string fileName)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            bool? showDialog = ofd.ShowDialog();

            if (showDialog.HasValue && showDialog.Value)
            {
                fileName = ofd.FileName;
                return true;
            }

            fileName = null;
            return showDialog;
        }

        #endregion

        private class AppenderSaveStrategy : ISaveStrategy
        {
            private readonly IElementConfiguration mConfiguration;

            public AppenderSaveStrategy(IElementConfiguration configuration)
            {
                mConfiguration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            }

            public void Execute()
            {
                if (mConfiguration.OriginalNode == null)
                {
                    //New node - add
                    mConfiguration.Log4NetNode.AppendChild(mConfiguration.NewNode);
                }
                else
                {
                    //Edit - replace
                    mConfiguration.Log4NetNode.ReplaceChild(mConfiguration.NewNode, mConfiguration.OriginalNode);
                }
            }
        }
    }
}
