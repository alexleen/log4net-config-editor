// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Windows;
using System.Xml;
using Editor.Enums;
using Editor.Models;
using log4net.Core;

namespace Editor.Windows
{
    /// <summary>
    /// Interaction logic for FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : Window
    {
        private readonly FilterModel mFilterModel;
        private readonly XmlNode mAppenderNode;
        private readonly XmlDocument mConfigXml;

        private readonly IDictionary<string, Level> mLevelsByName = new Dictionary<string, Level>
        {
            { Level.Off.Name, Level.Off },
            { Level.Debug.Name, Level.Debug },
            { Level.Info.Name, Level.Info },
            { Level.Notice.Name, Level.Notice },
            { Level.Warn.Name, Level.Warn },
            { Level.Error.Name, Level.Error },
            { Level.Severe.Name, Level.Severe },
            { Level.Critical.Name, Level.Critical },
            { Level.Alert.Name, Level.Alert },
            { Level.Fatal.Name, Level.Fatal },
            { Level.Emergency.Name, Level.Emergency },
            { Level.All.Name, Level.All }
        };

        public FilterWindow(Window owner, FilterModel filterModel, XmlNode appenderNode, XmlDocument configXml)
        {
            InitializeComponent();
            Owner = owner;
            mFilterModel = filterModel;
            mAppenderNode = appenderNode;
            mConfigXml = configXml;
            PopulateComboBoxes();
            Configure();
            Load();
        }

        private void PopulateComboBoxes()
        {
            IEnumerable<string> levelNames = mLevelsByName.Keys;

            xLevelToMatchComboBox.ItemsSource = levelNames;
            xMinLevelComboBox.ItemsSource = levelNames;
            xMaxLevelComboBox.ItemsSource = levelNames;
        }

        private void Configure()
        {
            GridLength zeroGridLength = new GridLength(0);

            switch (mFilterModel.Descriptor.Type)
            {
                case FilterType.DenyAll:
                    throw new InvalidOperationException("The filter window should not be shown for a Deny All filter.");
                case FilterType.LevelRange:
                    xLoggerToMatchRow.Height = zeroGridLength;
                    xLevelToMatchRow.Height = zeroGridLength;
                    xRegexToMatchRow.Height = zeroGridLength;
                    xStringToMatchRow.Height = zeroGridLength;
                    break;
                case FilterType.LevelMatch:
                    xLoggerToMatchRow.Height = zeroGridLength;
                    xMinLevelRow.Height = zeroGridLength;
                    xMaxLevelRow.Height = zeroGridLength;
                    xRegexToMatchRow.Height = zeroGridLength;
                    xStringToMatchRow.Height = zeroGridLength;
                    break;
                case FilterType.LoggerMatch:
                    xLevelToMatchRow.Height = zeroGridLength;
                    xMinLevelRow.Height = zeroGridLength;
                    xMaxLevelRow.Height = zeroGridLength;
                    xRegexToMatchRow.Height = zeroGridLength;
                    xStringToMatchRow.Height = zeroGridLength;
                    break;
                case FilterType.Mdc:
                    break;
                case FilterType.Ndc:
                    break;
                case FilterType.Property:
                    break;
                case FilterType.String:
                    xLoggerToMatchRow.Height = zeroGridLength;
                    xLevelToMatchRow.Height = zeroGridLength;
                    xMinLevelRow.Height = zeroGridLength;
                    xMaxLevelRow.Height = zeroGridLength;
                    xRegexToMatchRow.Height = zeroGridLength;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Load()
        {
            if (mFilterModel.Node == null)
            {
                return;
            }

            foreach (XmlNode childNode in mFilterModel.Node.ChildNodes)
            {
                string value = childNode.Attributes?["value"]?.Value;

                switch (childNode.Name)
                {
                    case "loggerToMatch":
                        xLoggerToMatchTextBox.Text = value;
                        break;
                    case "levelMatch":
                        xLevelToMatchComboBox.SelectedItem = value;
                        break;
                    case "levelMin":
                        xMinLevelComboBox.SelectedItem = value;
                        break;
                    case "levelMax":
                        xMaxLevelComboBox.SelectedItem = value;
                        break;
                    case "regexToMatch":
                        xRegexToMatchTextBox.Text = value;
                        break;
                    case "stringToMatch":
                        xStringToMatchTextBox.Text = value;
                        break;
                }
            }
        }

        private void SaveOnClick(object sender, RoutedEventArgs e)
        {
            if (mFilterModel.Node == null) //New filter
            {
                XmlAttribute filterTypeAttribute = mConfigXml.CreateAttribute("type");
                filterTypeAttribute.Value = mFilterModel.Descriptor.TypeNamespace;

                XmlElement filterElement = mConfigXml.CreateElement("filter");
                filterElement.Attributes.Append(filterTypeAttribute);

                mAppenderNode.AppendChild(filterElement);
            }
            
            Close();
        }

        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
