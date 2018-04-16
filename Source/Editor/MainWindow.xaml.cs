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
using Editor.Windows;
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
                AppenderDescriptor.File,
                AppenderDescriptor.RollingFile,
                AppenderDescriptor.EventLog,
                AppenderDescriptor.Async
            };
        }

        private void MainWindowOnLoaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Settings.Default.LastConfig))
            {
                Load(Settings.Default.LastConfig);
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
                Load(ofd.FileName);
            }
        }

        private void ReloadOnClick(object sender, RoutedEventArgs e)
        {
            Load(Settings.Default.LastConfig);
        }

        private void SaveOnClick(object sender, RoutedEventArgs e)
        {
            using (XmlTextWriter xtw = new XmlTextWriter(Settings.Default.LastConfig, Encoding.UTF8) { Formatting = Formatting.Indented })
            {
                mConfigXml.Save(xtw);
            }
        }

        private void Load(string fileName)
        {
            xConfigTextBox.Text = fileName;

            mConfigXml = new XmlDocument();
            mConfigXml.Load(fileName);

            //TODO if > 1 node abort
            mLog4NetNode = mConfigXml.SelectSingleNode("//log4net");

            if (mLog4NetNode == null)
            {
                MessageBox.Show(this, "Could not find log4net configuration.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //TODO does this only select appenders under the log4net node or the whole file?
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
            string type = appender.Attributes?["type"].Value;

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

        private void OpenAppenderWindow(AppenderType appenderType, XmlNode appenderNode = null)
        {
            switch (appenderType)
            {
                case AppenderType.Console:
                    break;
                case AppenderType.File:
                    AddFileAppenderWindow afaw = new AddFileAppenderWindow(this, appenderNode);
                    afaw.ShowDialog();
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
        }
    }
}
