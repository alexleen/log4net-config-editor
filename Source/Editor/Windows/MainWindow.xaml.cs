// Copyright © 2020 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using Editor.Definitions.Factory;
using Editor.Descriptors;
using Editor.Descriptors.Base;
using Editor.Enums;
using Editor.HistoryManager;
using Editor.Interfaces;
using Editor.Models;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private readonly IMessageBoxService mMessageBoxService;
        private readonly IToastService mToastService;
        private readonly IHistoryManager mConfigHistoryManager;
        private readonly IConfigurationFactory mConfigurationFactory;

        private IConfigurationXml mConfig;

        public IConfigurationXml ConfigurationXml
        {
            get => mConfig;
            private set
            {
                mConfig = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
            : base("MainWindowPlacement")
        {
            InitializeComponent();

            DataContext = this;

            mMessageBoxService = new MessageBoxService(this);
            mToastService = new ToastService();
            mConfigHistoryManager = new HistoryManagerFactory(new SettingManager<string>()).CreateConfigHistoryManager();
            mConfigurationFactory = new ConfigurationFactory(mToastService);

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

            xThresholdComboBox.ItemsSource = Log4NetUtilities.LevelsByName.Values;

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

            //Subscribe after RefreshConfigComboBox above is called to avoid an additional load
            xConfigComboBox.SelectionChanged += ConfigComboBoxOnSelectionChanged;
        }

        private void PasteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Paste();
        }

        private void Paste()
        {
            string text = Clipboard.GetText();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(text);
                XmlNode imported = mConfig.ConfigXml.ImportNode(doc.FirstChild, true);

                ModelCreateResult result = ModelFactory.TryCreate(imported, mConfig.Log4NetNode, out ModelBase model);

                if (result == ModelCreateResult.Success)
                {
                    OpenElementWindow(model, true);
                    return;
                }

                if (result == ModelCreateResult.UnknownAppender)
                {
                    mToastService.ShowError("Unrecognized appender.");
                    return;
                }
            }
            catch
            {
                // ignored
            }

            mToastService.ShowError("Unrecognized log4net element");
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
                ConfigurationXml = mConfigurationFactory.Create(sfd.FileName);
                ConfigurationXml.Load();
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
            else
            {
                mToastService.ShowInformation("No configuration file selected");
            }
        }

        private void OpenHereOnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Config Files (*.xml, *.config) | *.xml; *.config" };

            if (ofd.ShowDialog(this).IsTrue())
            {
                RefreshConfigComboBox(ofd.FileName);
                LoadFromFile(ofd.FileName);
            }
        }

        private void OpenInExplorer(object sender, RoutedEventArgs e)
        {
            string selectedConfig = (string)xConfigComboBox.SelectedItem;

            if (!string.IsNullOrEmpty(selectedConfig))
            {
                if (File.Exists(selectedConfig))
                {
                    Process.Start("explorer.exe", $"/select, \"{selectedConfig}\"");
                }
                else
                {
                    mMessageBoxService.ShowWarning("File has not been saved yet and therefore cannot be opened.");
                }
            }
            else
            {
                mToastService.ShowInformation("No configuration file selected");
            }
        }

        private void SaveCopyClick(object sender, RoutedEventArgs e)
        {
            if (mConfig != null)
            {
                SaveFileDialog sfw = new SaveFileDialog { Filter = "Config Files (*.xml, *.config) | *.xml; *.config" };

                if (sfw.ShowDialog(this).IsTrue())
                {
                    mConfig.SaveAsync(sfw.FileName);
                }
            }
            else
            {
                mToastService.ShowInformation("No configuration file selected");
            }
        }

        private void PasteClick(object sender, RoutedEventArgs e)
        {
            Paste();
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
            SaveToFileAsync();
        }

        private void SandAndCloseOnClick(object sender, RoutedEventArgs e)
        {
            SaveToFileAsync();
            Close();
        }

        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Writes <see cref="ConfigurationXml"/> to disk at currently selected config location.
        /// </summary>
        private async void SaveToFileAsync()
        {
            try
            {
                await ConfigurationXml.SaveAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"An unexpected error occurred while saving:{Environment.NewLine}{Environment.NewLine}{ex.Message}", "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReloadFromFile()
        {
            if (xConfigComboBox.SelectedItem == null)
            {
                //Reload was pressed without ever opening a config file
                mToastService.ShowInformation("No configuration file selected");
                return;
            }

            LoadFromFile((string)xConfigComboBox.SelectedItem);
        }

        private void LoadFromRam()
        {
            ConfigurationXml.Reload();
        }

        /// <summary>
        /// Replaces <see cref="ConfigurationXml"/> with a new instance for the specified file.
        /// </summary>
        /// <param name="filename"></param>
        private void LoadFromFile(string filename)
        {
            ConfigurationXml = mConfigurationFactory.Create(filename);

            try
            {
                ConfigurationXml.Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"An unexpected error occurred while loading '{filename}':{Environment.NewLine}{Environment.NewLine}{ex.Message}", "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            xChildren.ItemsSource = ConfigurationXml.Children;
            xAddRefsToMenuItem.ItemsSource = ConfigurationXml.Children.OfType<IAcceptAppenderRef>().Cast<NamedModel>();
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
                ConfigurationXml.RemoveChild(modelBase);
            }
        }

        private void RemoveRefsOnClick(object sender, RoutedEventArgs e)
        {
            foreach (ModelBase modelBase in xChildren.SelectedItems)
            {
                if (modelBase is AppenderModel appenderModel)
                {
                    ConfigurationXml.RemoveRefsTo(appenderModel);
                }
            }

            LoadFromRam();
        }

        private void AddRefsButtonOnItemClick(object sender, RoutedEventArgs e)
        {
            ModelBase destination = (ModelBase)((FrameworkElement)sender).DataContext;

            foreach (AppenderModel appenderModel in xChildren.SelectedItems.OfType<AppenderModel>())
            {
                XmlUtilities.AddAppenderRefToNode(ConfigurationXml.ConfigXml, destination.Node, appenderModel.Name);
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

        private void OpenElementWindow(ModelBase model, bool forceAppend = false)
        {
            OpenElementWindow(model, model.Node.Name, model.Descriptor, forceAppend);
        }

        private void OpenElementWindow(ModelBase model, string elementName, DescriptorBase descriptor, bool forceAppend = false)
        {
            IElementConfiguration configuration = ConfigurationXml.CreateElementConfigurationFor(model, elementName);

            ElementWindow elementWindow = new ElementWindow(configuration,
                                                            DefinitionFactory.Create(descriptor, configuration),
                                                            WindowSizeLocationFactory.Create(descriptor),
                                                            new AppendReplaceSaveStrategy(configuration, forceAppend))
                { Owner = this };
            elementWindow.ShowDialog();
            LoadFromRam();
        }

        private void CopyElementToClipboard(object sender, RoutedEventArgs e)
        {
            ModelBase model = (ModelBase)xChildren.SelectedItem;

            Clipboard.SetText(model.Node.OuterXml);

            mToastService.ShowSuccess("XML copied to clipboard");
        }

        private void OpenLogFileClick(object sender, RoutedEventArgs e)
        {
            string filePath = GetFilePath(sender);

            if (File.Exists(filePath))
            {
                Process.Start(filePath);
            }
            else
            {
                mToastService.ShowInformation("Log file does not exist");
            }
        }

        private void OpenLogFolderClick(object sender, RoutedEventArgs e)
        {
            string filePath = GetFilePath(sender);
            string dirPath = Path.GetDirectoryName(filePath);

            if (File.Exists(filePath))
            {
                Process.Start("explorer.exe", $"/select, \"{filePath}\"");
            }
            else if (Directory.Exists(dirPath))
            {
                Process.Start(dirPath);
            }
            else
            {
                mToastService.ShowInformation("Log file and directory do not exist");
            }
        }

        private string GetFilePath(object sender)
        {
            AppenderModel appender = (AppenderModel)xChildren.SelectedItem;

            IElementConfiguration configuration = mConfig.CreateElementConfigurationFor(appender, appender.Node.Name);

            IElementDefinition elementDefinition = DefinitionFactory.Create(appender.Descriptor, configuration);

            elementDefinition.Initialize();

            foreach (IProperty prop in elementDefinition.Properties.ToList())
            {
                prop.Load(appender.Node);
            }

            return elementDefinition.Properties.OfType<ConfigProperties.File>().Single().FilePath;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ContextMenuOpened(object sender, RoutedEventArgs e)
        {
            ModelBase model = (ModelBase)xChildren.SelectedItem;
            xOpenLogFileMenuItem.IsEnabled = model.Descriptor == AppenderDescriptor.File;
            xOpenLogFolderMenuItem.IsEnabled = model.Descriptor == AppenderDescriptor.File || model.Descriptor == AppenderDescriptor.RollingFile;
        }
    }
}
