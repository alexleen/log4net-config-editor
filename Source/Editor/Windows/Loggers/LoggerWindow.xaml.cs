// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Enums;
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
            }
        }

        private void SaveOnClick(object sender, RoutedEventArgs e)
        {
            XmlNode rootLoggerNode = mChangeType == ChangeType.Add ? mRootLoggerNode : mConfigXml.CreateElement("root");
            mConfigXml.CreateElementWithAttribute("level", "value", (string)xLevelsComboBox.SelectedItem).AppentTo(rootLoggerNode);

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
