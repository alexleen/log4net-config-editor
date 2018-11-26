// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Editor.Definitions.Factory;
using Editor.Descriptors;
using Editor.Descriptors.Base;
using Editor.Enums;
using Editor.HistoryManager;
using Editor.Interfaces;
using Editor.Models.Base;
using Editor.Models.ConfigChildren;
using Editor.SaveStrategies;
using Editor.Utilities;
using Editor.Windows.SizeLocation;
using Editor.XML;
using Microsoft.Win32;

namespace Editor.Windows
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly IMessageBoxService mMessageBoxService;
        private readonly HistoryManager.HistoryManager mConfigHistoryManager;
        private readonly IConfigurationFactory mConfigurationFactory;
        private IConfigurationXml mConfigurationXml;

        public MainWindow()
            : base("MainWindowPlacement")
        {
            InitializeComponent();

            mMessageBoxService = new MessageBoxService(this);
            mConfigHistoryManager = new HistoryManager.HistoryManager("HistoricalConfigs", new SettingManager<string>());
            mConfigurationFactory = new ConfigurationFactory(mMessageBoxService);

            xAddAppenderButton.ItemsSource = new[]
            {
                AppenderDescriptor.Console,
                AppenderDescriptor.File,
                AppenderDescriptor.RollingFile,
                AppenderDescriptor.EventLog,
                AppenderDescriptor.Async,
                AppenderDescriptor.Forwarding,
                AppenderDescriptor.ManagedColor,
                AppenderDescriptor.Udp,
                AppenderDescriptor.LocalSyslog,
                AppenderDescriptor.RemoteSyslog
            };

            xUpdateComboBox.ItemsSource = new[] { Update.Merge, Update.Overwrite };

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

        private void NewClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog { Filter = "XML Config File|*.xml" };

            bool? result = sfd.ShowDialog(this);

            if (result.IsTrue())
            {
                RefreshConfigComboBox(sfd.FileName);
                mConfigurationXml = mConfigurationFactory.Create(sfd.FileName);
                mConfigurationXml.Load();
            }
        }

        private void OpenThereOnClick(object sender, RoutedEventArgs e)
        {
            string selectedConfig = (string)xConfigComboBox.SelectedItem;

            if (!string.IsNullOrEmpty(selectedConfig))
            {
                if (File.Exists(selectedConfig))
                {
                    Process.Start(selectedConfig);
                }
                else
                {
                    mMessageBoxService.ShowWarning("File has not been saved yet and therefore cannot be opened.");
                }
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
        /// Performs the following actions:<para/>
        /// 1. Save the specified file name to the set of historical configs.<para/>
        /// 2. Sets the config ComboBox's ItemsSource to the set of historical configs.<para/>
        /// 3. Sets the config ComboBox's to the specified filename.
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

        /// <summary>
        /// Saves root attributes to <see cref="mConfigurationXml"/> then writes <see cref="mConfigurationXml"/> to disk at currently selected config location.
        /// </summary>
        private void SaveToFile()
        {
            mConfigurationXml.Debug = xDebugCheckBox.IsChecked.IsTrue();
            mConfigurationXml.Update = (Update)xUpdateComboBox.SelectedItem;
            mConfigurationXml.Threshold = Log4NetUtilities.LevelsByName[(string)xThresholdComboBox.SelectedItem];
            mConfigurationXml.Save();
        }

        private void ReloadFromFile()
        {
            if (xConfigComboBox.SelectedItem == null)
            {
                //Reload was pressed without ever opening a config file
                return;
            }

            LoadFromFile((string)xConfigComboBox.SelectedItem);
        }

        private void LoadFromRam()
        {
            mConfigurationXml.Reload();
        }

        /// <summary>
        /// Replaces <see cref="mConfigurationXml"/> with a new instance for the specified file.
        /// </summary>
        /// <param name="filename"></param>
        private void LoadFromFile(string filename)
        {
            mConfigurationXml = mConfigurationFactory.Create(filename);
            mConfigurationXml.Load();

            xDebugCheckBox.IsChecked = mConfigurationXml.Debug;
            xUpdateComboBox.SelectedItem = mConfigurationXml.Update;
            xThresholdComboBox.SelectedItem = mConfigurationXml.Threshold.Name;
            xChildren.ItemsSource = mConfigurationXml.Children;
            xAddRefsButton.ItemsSource = mConfigurationXml.Children.OfType<IAcceptAppenderRef>().Cast<NamedModel>();
            xRightSp.IsEnabled = true;
            xSaveButton.IsEnabled = true;
            xSaveAndCloseButton.IsEnabled = true;
        }

        private void AddAppenderItemOnClick(object appender)
        {
            OpenElementWindow((AppenderDescriptor)appender);
        }

        private void AddRootClick(object sender, RoutedEventArgs e)
        {
            if (xChildren.ItemsSource.Cast<ModelBase>().Any(cm => cm.Node.Name == "root"))
            {
                MessageBox.Show(this, "This configuration already contains a root logger.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            OpenElementWindow(LoggerDescriptor.Root);
        }

        private void AddLoggerClick(object sender, RoutedEventArgs e)
        {
            OpenElementWindow(LoggerDescriptor.Logger);
        }

        private void AddRendererClick(object sender, RoutedEventArgs e)
        {
            OpenElementWindow(RendererDescriptor.Renderer);
        }

        private void AddParamClick(object sender, RoutedEventArgs e)
        {
            OpenElementWindow(ParamDescriptor.Param);
        }

        private void RemoveAppenderOnClick(object sender, RoutedEventArgs e)
        {
            foreach (ModelBase modelBase in xChildren.SelectedItems.OfType<ModelBase>().ToList())
            {
                mConfigurationXml.RemoveChild(modelBase);
            }
        }

        private void RemoveRefsOnClick(object sender, RoutedEventArgs e)
        {
            foreach (ModelBase modelBase in xChildren.SelectedItems)
            {
                if (modelBase is AppenderModel appenderModel)
                {
                    mConfigurationXml.RemoveRefsTo(appenderModel);
                }
            }

            LoadFromRam();
        }

        private void AddRefsButtonOnItemClick(object obj)
        {
            ModelBase destination = (ModelBase)obj;

            foreach (AppenderModel appenderModel in xChildren.SelectedItems.OfType<AppenderModel>())
            {
                XmlUtilities.AddAppenderRefToNode(mConfigurationXml.ConfigXml, destination.Node, appenderModel.Name);
            }

            LoadFromRam();
        }

        private void EditAppenderOnClick(object sender, RoutedEventArgs e)
        {
            OpenElementWindow((ModelBase)((DataGridRow)sender).DataContext);
        }

        private void OpenElementWindow(DescriptorBase descriptor)
        {
            OpenElementWindow(null, descriptor.ElementName, descriptor);
        }

        private void OpenElementWindow(ModelBase model)
        {
            OpenElementWindow(model, model.Node.Name, model.Descriptor);
        }

        private void OpenElementWindow(ModelBase model, string elementName, DescriptorBase descriptor)
        {
            IElementConfiguration configuration = mConfigurationXml.CreateElementConfigurationFor(model, elementName);

            ElementWindow elementWindow = new ElementWindow(configuration,
                                                            DefinitionFactory.Create(descriptor, configuration),
                                                            WindowSizeLocationFactory.Create(descriptor),
                                                            new AppendReplaceSaveStrategy(configuration))
                { Owner = this };
            elementWindow.ShowDialog();
            LoadFromRam();
        }
    }
}
