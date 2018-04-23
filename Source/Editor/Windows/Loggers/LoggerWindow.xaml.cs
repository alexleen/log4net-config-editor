// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml;
using Editor.Enums;
using Editor.Models;
using Editor.Utilities;

namespace Editor.Windows.Loggers
{
    /// <summary>
    /// Interaction logic for LoggerWindow.xaml
    /// </summary>
    public partial class LoggerWindow : Window
    {
        private readonly XmlDocument mConfigXml;
        private readonly XmlNode mLog4NetNode;
        private readonly XmlNode mRootLoggerNode;
        private readonly ChangeType mChangeType;

        public LoggerWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode rootLoggerNode, ChangeType changeType)
        {
            InitializeComponent();
            Owner = owner;
            Title = "Logger";
            mConfigXml = configXml;
            mLog4NetNode = log4NetNode;
            mRootLoggerNode = rootLoggerNode;
            mChangeType = changeType;

            xLevelsComboBox.ItemsSource = Log4NetUtilities.LevelsByName.Keys;
        }

        private void LoggerWindowOnLoaded(object sender, RoutedEventArgs e)
        {
            if (mChangeType == ChangeType.Edit)
            {
                string level = mRootLoggerNode["level"]?.Attributes["value"].Value;

                if (!string.IsNullOrEmpty(level))
                {
                    xLevelsComboBox.SelectedItem = level;
                }

                XmlNodeList appenderRefs = mRootLoggerNode.SelectNodes("appender-ref");

                if (appenderRefs != null)
                {
                    ICollection<AppenderRefModel> appenderRefModels = new List<AppenderRefModel>();

                    foreach (XmlNode appenderRef in appenderRefs)
                    {
                        string refValue = appenderRef.Attributes["ref"].Value;
                        appenderRefModels.Add(new AppenderRefModel(refValue));
                    }

                    xRefsListBox.ItemsSource = appenderRefModels;
                }
            }
        }

        private void SaveOnClick(object sender, RoutedEventArgs e)
        {
            XmlNode rootLoggerNode = mChangeType == ChangeType.Add ? mRootLoggerNode : mConfigXml.CreateElement("root");
            mConfigXml.CreateElementWithAttribute("level", "value", (string)xLevelsComboBox.SelectedItem).AppendTo(rootLoggerNode);

            foreach (AppenderRefModel appenderRefModel in xRefsListBox.ItemsSource.Cast<AppenderRefModel>().Where(appenderRefModel => appenderRefModel.IsEnabled))
            {
                mConfigXml.CreateElementWithAttribute("appender-ref", "ref", appenderRefModel.Ref).AppendTo(rootLoggerNode);
            }

            if (mChangeType == ChangeType.Add)
            {
                mLog4NetNode.AppendChild(rootLoggerNode);
            }
            else
            {
                mLog4NetNode.ReplaceChild(rootLoggerNode, mRootLoggerNode);
            }

            Close();
        }

        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
