using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;
using Editor.Properties;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using Microsoft.Win32;

namespace Editor
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
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
            OpenFileDialog ofd = new OpenFileDialog {Filter = "Config Files (*.xml, *.config) | *.xml; *.config"};

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
            throw new NotImplementedException();
        }

        private void Load(string fileName)
        {
            xConfigTextBox.Text = fileName;

            XmlDocument configXml = new XmlDocument();
            configXml.Load(fileName);

            XmlNode log4netNode = configXml.SelectSingleNode("//log4net");

            if (log4netNode == null)
            {
                MessageBox.Show(this, "Could not fin log4net configuration.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            XmlNodeList appenderList = log4netNode.SelectNodes("appender");

            if (appenderList != null)
            {
                ICollection<AppenderModel> appenderModels = new List<AppenderModel>();

                foreach (XmlNode node in appenderList)
                {
                    AppenderModel appenderModel = Create(node);
                    appenderModels.Add(appenderModel);
                    IEnumerable<XmlNode> appenderRefs = FindAppenderRefs(log4netNode, appenderModel.Name);
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

        private IEnumerable<XmlNode> FindAppenderRefs(XmlNode log4netNode, string appenderName)
        {
            XmlNodeList appenderRefs = log4netNode.SelectNodes("//appender-ref");

            foreach (XmlNode appenderRef in appenderRefs)
            {
                if (appenderRef.Attributes?["ref"].Value == appenderName)
                {
                    yield return appenderRef;
                }
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
    }
}