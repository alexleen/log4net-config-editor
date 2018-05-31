// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Editor.Descriptors;
using Editor.Enums;
using Editor.HistoryManager;
using Editor.Models;
using Editor.Utilities;
using Editor.Windows.Appenders;
using Editor.Windows.Loggers;
using Microsoft.Win32;

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
                AppenderDescriptor.Async
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

        private void OpenOnClick(object sender, RoutedEventArgs e)
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

            return unrecognized;
        }

        private static bool TryCreate(XmlNode appender, out AppenderModel appenderModel)
        {
            string type = appender.Attributes?["type"]?.Value;

            if (AppenderDescriptor.TryFindByTypeNamespace(type, out AppenderDescriptor descriptor))
            {
                appenderModel = new AppenderModel(descriptor, appender);
                return true;
            }

            appenderModel = null;
            return false;
        }

        private void AddAppenderItemOnClick(object appender)
        {
            OpenAppenderWindow(((AppenderDescriptor)appender).Type, null);
        }

        private void EditAppenderOnClick(object sender, RoutedEventArgs e)
        {
            object dataContext = ((Button)sender).DataContext;

            if (dataContext is AppenderModel appenderModel)
            {
                OpenAppenderWindow(appenderModel.Descriptor.Type, appenderModel.Node);
            }
            else if (dataContext is ChildModel childModel)
            {
                OpenLoggerWindow(ChangeType.Edit, childModel.Node);
            }
        }

        private void RemoveAppenderOnClick(object sender, RoutedEventArgs e)
        {
            ChildModel childModel = (ChildModel)((Button)sender).DataContext;
            mLog4NetNode.RemoveChild(childModel.Node);

            if (childModel is AppenderModel appenderModel)
            {
                //Remove all appender refs
                foreach (RefModel refModel in XmlUtilities.FindAppenderRefs(mLog4NetNode, appenderModel.Name))
                {
                    refModel.AppenderRef.ParentNode?.RemoveChild(refModel.AppenderRef);
                }
            }

            LoadFromRam();
        }

        private void OpenAppenderWindow(AppenderType appenderType, XmlNode appenderNode)
        {
            AppenderWindow appenderWindow;

            switch (appenderType)
            {
                case AppenderType.Console:
                    appenderWindow = new ConsoleAppenderWindow(this, mConfigXml, mLog4NetNode, appenderNode);
                    break;
                case AppenderType.File:
                    appenderWindow = new FileAppenderWindow(this, mConfigXml, mLog4NetNode, appenderNode);
                    break;
                case AppenderType.RollingFile:
                    appenderWindow = new RollingFileAppenderWindow(this, mConfigXml, mLog4NetNode, appenderNode);
                    break;
                case AppenderType.EventLog:
                    appenderWindow = new EventLogAppenderWindow(this, mConfigXml, mLog4NetNode, appenderNode);
                    break;
                case AppenderType.Async:
                    appenderWindow = new AsyncAppenderWindow(this, mConfigXml, mLog4NetNode, appenderNode);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(appenderType), appenderType, null);
            }

            appenderWindow.ShowDialog();
            LoadFromRam();
        }

        private void AddLoggerOnClick(object sender)
        {
            if (xChildren.ItemsSource.Cast<ChildModel>().Any(cm => cm.ElementName == "root"))
            {
                MessageBox.Show(this, "This configuration already contains a root logger.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            OpenLoggerWindow(ChangeType.Add, mConfigXml.CreateElement("root"));
        }

        private void OpenLoggerWindow(ChangeType changeType, XmlNode rootLoggerNode)
        {
            LoggerWindow lw = new LoggerWindow(this, mConfigXml, mLog4NetNode, rootLoggerNode, changeType);
            lw.ShowDialog();
            LoadFromRam();
        }
    }
}
