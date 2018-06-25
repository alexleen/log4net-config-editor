// Copyright © 2018 Alex Leendertsen

using Editor.Definitions.Factory;
using Editor.Descriptors;
using Editor.Descriptors.Base;
using Editor.HistoryManager;
using Editor.Interfaces;
using Editor.Models;
using Editor.Utilities;
using Editor.Windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace Editor
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private XmlDocument mConfigXml;
        private XmlNode mLog4NetNode;
        private readonly HistoryManager.HistoryManager mConfigHistoryManager;

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

            xAddLoggerButton.ItemsSource = new[]
            {
                LoggerDescriptor.Root
            };
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
            using (XmlTextWriter xtw = new XmlTextWriter((string)xConfigComboBox.SelectedItem, Encoding.UTF8) { Formatting = Formatting.Indented })
            {
                mConfigXml.Save(xtw);
            }
        }

        private void ReloadFromFile()
        {
            LoadFromFile((string)xConfigComboBox.SelectedItem);
        }

        private void LoadFromFile(string fileName)
        {
            mConfigXml = new XmlDocument();
            mConfigXml.Load(fileName);

            bool? unrecognizedAppender = LoadFromRam();

            if (unrecognizedAppender.HasValue && unrecognizedAppender.Value)
            {
                MessageBox.Show(this, "At least one unrecognized appender was found in this configuration.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Loads current state of <see cref="mConfigXml"/> into view.
        /// Overwrites any existing values in the view not saved to <see cref="mConfigXml"/>.
        /// Returns true if an unrecognized/unsupported appender was found.
        /// Returns null if configuration can not be loaded.
        /// </summary>
        /// <returns></returns>
        private bool? LoadFromRam()
        {
            XmlNodeList log4NetNodes = mConfigXml.SelectNodes("//log4net");

            if (log4NetNodes == null || log4NetNodes.Count == 0)
            {
                MessageBox.Show(this, "Could not find log4net configuration.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            if (log4NetNodes.Count > 1)
            {
                MessageBox.Show(this, "More than one 'log4net' element was found in the specified file. Using the first occurrence.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            mLog4NetNode = log4NetNodes[0];

            ICollection<ChildModel> children = new List<ChildModel>();

            //Only selects appenders under this log4net element
            XmlNodeList appenderList = mLog4NetNode.SelectNodes("appender");

            bool unrecognized = false;

            if (appenderList != null)
            {
                foreach (XmlNode node in appenderList)
                {
                    if (TryCreate(node, out AppenderModel model))
                    {
                        children.Add(model);
                    }
                    else
                    {
                        unrecognized = true;
                    }
                }
            }

            XmlNode root = mLog4NetNode.SelectSingleNode("root");

            if (root != null)
            {
                children.Add(new ChildModel("root", root));
            }

            xChildren.ItemsSource = children;

            xAddRefsButton.ItemsSource = XmlUtilities.FindAvailableAppenderRefLocations(mLog4NetNode);

            return unrecognized;
        }

        private bool TryCreate(XmlNode appender, out AppenderModel appenderModel)
        {
            string type = appender.Attributes?["type"]?.Value;

            if (AppenderDescriptor.TryFindByTypeNamespace(type, out AppenderDescriptor descriptor))
            {
                string name = appender.Attributes?["name"].Value;
                int incomingReferences = mLog4NetNode.SelectNodes($"//appender-ref[@ref='{name}']").Count;
                appenderModel = new AppenderModel(descriptor, appender, name, incomingReferences);
                return true;
            }

            appenderModel = null;
            return false;
        }

        private void AddAppenderItemOnClick(object appender)
        {
            OpenElementWindow((AppenderDescriptor)appender, null, "appender");
        }

        private void EditAppenderOnClick(object sender, RoutedEventArgs e)
        {
            object dataContext = ((DataGridRow)sender).DataContext;

            if (dataContext is AppenderModel appenderModel)
            {
                OpenElementWindow(appenderModel.Descriptor, appenderModel.Node, "appender");
            }
            else if (dataContext is ChildModel childModel)
            {
                OpenElementWindow(LoggerDescriptor.Root, childModel.Node, "root");
            }
        }

        private void RemoveAppenderOnClick(object sender, RoutedEventArgs e)
        {
            foreach (ChildModel childModel in xChildren.SelectedItems)
            {
                mLog4NetNode.RemoveChild(childModel.Node);

                if (childModel is AppenderModel appenderModel)
                {
                    RemoveRefsTo(appenderModel);
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
                    RemoveRefsTo(appenderModel);
                }
            }

            LoadFromRam();
        }

        private void RemoveRefsTo(AppenderModel appenderModel)
        {
            //Remove all appender refs
            foreach (RefModel refModel in XmlUtilities.FindAppenderRefs(mLog4NetNode, appenderModel.Name))
            {
                refModel.AppenderRef.ParentNode?.RemoveChild(refModel.AppenderRef);
            }
        }

        private void AddRefsButtonOnItemClick(object obj)
        {
            ChildModel destination = (ChildModel)obj;

            foreach (AppenderModel appenderModel in xChildren.SelectedItems.OfType<AppenderModel>())
            {
                XmlUtilities.AddAppenderRefToNode(mConfigXml, destination.Node, appenderModel.Name);
            }

            LoadFromRam();
        }

        private void OpenElementWindow(DescriptorBase descriptor, XmlNode appenderNode, string elementName)
        {
            IElementConfiguration appenderConfiguration = new ElementConfiguration(mConfigXml, mLog4NetNode, appenderNode, mConfigXml.CreateElement(elementName));
            ElementWindow elementWindow = new ElementWindow(appenderConfiguration,
                                                            DefinitionFactory.Create(descriptor, appenderConfiguration),
                                                            WindowSizeLocationFactory.Create(descriptor),
                                                            new AppenderSaveStrategy(appenderConfiguration));
            elementWindow.ShowDialog();
            LoadFromRam();
        }

        private void AddLoggerOnClick(object sender)
        {
            if (xChildren.ItemsSource.Cast<ChildModel>().Any(cm => cm.ElementName == "root"))
            {
                MessageBox.Show(this, "This configuration already contains a root logger.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            OpenElementWindow(LoggerDescriptor.Root, null, "root");
        }

        private class AppenderSaveStrategy : ISaveStrategy
        {
            private readonly IElementConfiguration mConfiguration;

            public AppenderSaveStrategy(IElementConfiguration mConfiguration)
            {
                this.mConfiguration = mConfiguration ?? throw new ArgumentNullException(nameof(mConfiguration));
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

    internal class ElementConfiguration : IElementConfiguration
    {
        public ElementConfiguration(XmlDocument xmlDocument, XmlNode log4NetNode, XmlNode originalNode, XmlNode newNode)
        {
            ConfigXml = xmlDocument;
            Log4NetNode = log4NetNode;
            OriginalNode = originalNode;
            NewNode = newNode;
        }

        public ElementConfiguration(IConfiguration configuration, XmlNode originalAppenderNode, XmlNode newAppenderNode)
            : this(configuration.ConfigXml, configuration.Log4NetNode, originalAppenderNode, newAppenderNode)
        {
        }

        public XmlNode OriginalNode { get; }

        public XmlNode NewNode { get; }

        public XmlDocument ConfigXml { get; }

        public XmlNode Log4NetNode { get; }
    }
}
