// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Editor.Models;
using Editor.Utilities;
using log4net.Core;

namespace Editor.Windows.Appenders
{
    /// <summary>
    /// Interaction logic for MappingWindow.xaml
    /// </summary>
    public partial class MappingWindow
    {
        public MappingWindow()
        {
            InitializeComponent();

            xLevelComboBox.ItemsSource = Log4NetUtilities.LevelsByName.Keys;
            xLevelComboBox.SelectedItem = Log4NetUtilities.LevelsByName.Keys.First();

            IEnumerable<ConsoleColor> consoleColors = Enum.GetValues(typeof(ConsoleColor)).Cast<ConsoleColor>();
            xForegroundComboBox.ItemsSource = consoleColors;
            xBackgroundComboBox.ItemsSource = consoleColors;
        }

        public MappingWindow(MappingModel mappingModelToEdit)
            : this()
        {
            xLevelComboBox.SelectedItem = mappingModelToEdit.Level.Name;
            xForegroundComboBox.SelectedItem = mappingModelToEdit.ForeColor;
            xBackgroundComboBox.SelectedItem = mappingModelToEdit.BackColor;
        }

        private void SaveOnClick(object sender, RoutedEventArgs e)
        {
            if (xForegroundComboBox.SelectedItem == null && xBackgroundComboBox.SelectedItem == null)
            {
                MessageBox.Show(this, "At least one mapping must be defined.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Result = (Log4NetUtilities.LevelsByName[(string)xLevelComboBox.SelectedItem],
                (ConsoleColor?)xForegroundComboBox.SelectedItem,
                (ConsoleColor?)xBackgroundComboBox.SelectedItem);

            Close();
        }

        /// <summary>
        /// User chosen values. Will be null if Save was not clicked.
        /// </summary>
        public (Level Level, ConsoleColor? ForeColor, ConsoleColor? BackColor) Result { get; private set; }

        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
