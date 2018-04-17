// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Windows;
using Editor.Descriptors;
using Editor.Enums;
using log4net.Core;

namespace Editor.Windows
{
    /// <summary>
    /// Interaction logic for FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : Window
    {
        private readonly FilterDescriptor mFilterDescriptor;

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

        public FilterWindow(Window owner, FilterDescriptor filterDescriptor)
        {
            InitializeComponent();
            Owner = owner;
            mFilterDescriptor = filterDescriptor;
            PopulateComboBoxes();
            Configure();
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

            switch (mFilterDescriptor.Type)
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

        private void SaveOnClick(object sender, RoutedEventArgs e)
        {
        }

        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
