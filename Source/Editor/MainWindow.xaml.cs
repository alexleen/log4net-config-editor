// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Editor.Descriptors;
using Editor.Enums;
using Editor.Models;
using Editor.Properties;
using Editor.Utilities;
using Editor.Windows;
using Editor.Windows.Appenders;
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

        public MainWindow()
        {
            InitializeComponent();

            xAppenderComboBox.ItemsSource = new[]
            {
                AppenderDescriptor.Console,
                AppenderDescriptor.File
                //AppenderDescriptor.RollingFile,
                //AppenderDescriptor.EventLog,
                //AppenderDescriptor.Async
            };
        }

        private void MainWindowOnLoaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Settings.Default.LastConfig))
            {
                LoadFromFile(Settings.Default.LastConfig);
            }
        }

        private void OpenOnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Config Files (*.xml, *.config) | *.xml; *.config" };

            bool? showDialog = ofd.ShowDialog(this);

            if (showDialog.Value)
            {
                Settings.Default.LastConfig = ofd.FileName;
                Settings.Default.Save();
                LoadFromFile(ofd.FileName);
            }
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
            using (XmlTextWriter xtw = new XmlTextWriter(Settings.Default.LastConfig, Encoding.UTF8) { Formatting = Formatting.Indented })
            {
                mConfigXml.Save(xtw);
            }
        }

        private void ReloadFromFile()
        {
            LoadFromFile(Settings.Default.LastConfig);
        }

        private void LoadFromFile(string fileName)
        {
            xConfigTextBox.Text = fileName;

            mConfigXml = new XmlDocument();
            mConfigXml.Load(fileName);

            LoadFromRam();
        }

        private void LoadFromRam()
        {
            XmlNodeList log4NetNodes = mConfigXml.SelectNodes("//log4net");

            if (log4NetNodes == null || log4NetNodes.Count == 0)
            {
                MessageBox.Show(this, "Could not find log4net configuration.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (log4NetNodes.Count > 1)
            {
                MessageBox.Show(this, "More than one 'log4net' element was found in the specified file. Using the first occurrence.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            mLog4NetNode = log4NetNodes[0];

            //Only selects appenders under this log4net element
            XmlNodeList appenderList = mLog4NetNode.SelectNodes("appender");

            if (appenderList != null)
            {
                ICollection<AppenderModel> appenderModels = new List<AppenderModel>();

                foreach (XmlNode node in appenderList)
                {
                    appenderModels.Add(Create(node));
                }

                xChildren.ItemsSource = appenderModels;
            }
        }

        private static AppenderModel Create(XmlNode appender)
        {
            string type = appender.Attributes?["type"]?.Value;

            if (AppenderDescriptor.TryFindByTypeNamespace(type, out AppenderDescriptor descriptor))
            {
                return new AppenderModel(descriptor, appender);
            }

            throw new InvalidOperationException("Unknown appender: " + type);
        }

        private void RefsOnClick(object sender, RoutedEventArgs e)
        {
            AppenderModel appenderModel = (AppenderModel)((Button)sender).DataContext;
            RefsWindow refsWindow = new RefsWindow(this, mConfigXml, mLog4NetNode, appenderModel.Name);
            refsWindow.ShowDialog();
        }

        private void AddAppenderOnClick(object sender, RoutedEventArgs e)
        {
            OpenAppenderWindow(((AppenderDescriptor)xAppenderComboBox.SelectionBoxItem).Type);
        }

        private void EditAppenderOnClick(object sender, RoutedEventArgs e)
        {
            AppenderModel appenderModel = (AppenderModel)((Button)sender).DataContext;
            OpenAppenderWindow(appenderModel.Descriptor.Type, appenderModel.Node);
        }

        private void RemoveAppenderOnClick(object sender, RoutedEventArgs e)
        {
            AppenderModel appenderModel = (AppenderModel)((Button)sender).DataContext;
            mLog4NetNode.RemoveChild(appenderModel.Node);

            //Remove all appender refs
            foreach (RefModel refModel in XmlUtilities.FindAppenderRefs(mLog4NetNode, appenderModel.Name))
            {
                refModel.AppenderRef.ParentNode.RemoveChild(refModel.AppenderRef);
            }

            LoadFromRam();
        }

        private void OpenAppenderWindow(AppenderType appenderType, XmlNode appenderNode = null)
        {
            switch (appenderType)
            {
                case AppenderType.Console:
                    ConsoleAppenderWindow caw = new ConsoleAppenderWindow(this, mConfigXml, mLog4NetNode, appenderNode);
                    caw.ShowDialog();
                    break;
                case AppenderType.File:
                    FileAppenderWindow faw = new FileAppenderWindow(this, mConfigXml, mLog4NetNode, appenderNode);
                    faw.ShowDialog();
                    break;
                case AppenderType.RollingFile:
                    break;
                case AppenderType.EventLog:
                    break;
                case AppenderType.Async:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(appenderType), appenderType, null);
            }

            LoadFromRam();
        }
    }
}
