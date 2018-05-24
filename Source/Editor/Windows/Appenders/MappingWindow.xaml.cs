// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Editor.Utilities;

namespace Editor.Windows.Appenders
{
    /// <summary>
    /// Interaction logic for MappingWindow.xaml
    /// </summary>
    public partial class MappingWindow : Window
    {
        public MappingWindow()
        {
            InitializeComponent();

            xLevelComboBox.ItemsSource = Log4NetUtilities.LevelsByName.Keys;

            IEnumerable<ConsoleColor> consoleColors = Enum.GetValues(typeof(ConsoleColor)).Cast<ConsoleColor>();
            xForegroundComboBox.ItemsSource = consoleColors;
            xBackgroundComboBox.ItemsSource = consoleColors;
        }

        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
