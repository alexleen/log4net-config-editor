// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
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

        //TODO no Save? Save on the fly?
        private void SaveOnClick(object sender, RoutedEventArgs e)
        {            
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

            //TODO does this only select appenders under the log4net node or the hole file?
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

        //TODO factory
        private static AppenderModel Create(XmlNode appender)
        {
            string type = appender.Attributes?["type"].Value;
            string name = appender.Attributes?["name"].Value;

            switch (type)
            {
                case "log4net.Appender.ConsoleAppender":
                    return new AppenderModel(true, "Console Appender", name);
                case "log4net.Appender.EventLogAppender":
                    return new AppenderModel(true, "Event Log Appender", name);
                case "log4net.Appender.FileAppender":
                    return new AppenderModel(true, "File Appender", name);
                case "Log4Net.Async.AsyncForwardingAppender,Log4Net.Async":
                    return new AppenderModel(true, "Async Forwarding Appender", name);
                default:
                    throw new ArgumentException($"Unsupported appender type: {appender.GetType().Name}");
            }
        }

        private class AppenderModel
        {
            public AppenderModel(bool enabled, string appenderType, string name)
            {
                Enabled = enabled;
                AppenderType = appenderType;
                Name = name;
            }

            public bool Enabled { get; set; }
            public string AppenderType { get; }
            public string Name { get; }
        }

        private void RefsOnClick(object sender, RoutedEventArgs e)
        {
            AppenderModel appenderModel = (AppenderModel)((Button)sender).DataContext;
            RefsWindow refsWindow = new RefsWindow(this, mConfigXml, mLog4NetNode, appenderModel.Name);
            refsWindow.ShowDialog();
        }
    }
}
