// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Editor.Models;
using Editor.Utilities;

namespace Editor.Windows.Appenders
{
    /// <summary>
    /// Interaction logic for MappingWindow.xaml
    /// </summary>
    public partial class MappingWindow
    {
        //private readonly MappingModel mMappingModel;
        private readonly Action<MappingModel> mOnAdd;
        private readonly Action<MappingModel> mOnRemove;

        public MappingWindow(Action<MappingModel> onAdd, Action<MappingModel> onRemove)
        {
            mOnAdd = onAdd;
            mOnRemove = onRemove;
            InitializeComponent();

            xLevelComboBox.ItemsSource = Log4NetUtilities.LevelsByName.Keys;
            xLevelComboBox.SelectedItem = Log4NetUtilities.LevelsByName.Keys.First();

            IEnumerable<ConsoleColor> consoleColors = Enum.GetValues(typeof(ConsoleColor)).Cast<ConsoleColor>();
            xForegroundComboBox.ItemsSource = consoleColors;
            xBackgroundComboBox.ItemsSource = consoleColors;
        }

        //public MappingWindow(MappingModel mappingModel)
        //    : this(null)
        //{
        //    mMappingModel = mappingModel;
        //    xLevelComboBox.SelectedItem = mappingModel.Level.Name;
        //    xForegroundComboBox.SelectedItem = mappingModel.ForeColor;
        //    xBackgroundComboBox.SelectedItem = mappingModel.BackColor;
        //}

        private void SaveOnClick(object sender, RoutedEventArgs e)
        {
            mOnAdd(new MappingModel(Log4NetUtilities.LevelsByName[(string)xLevelComboBox.SelectedItem],
                                    (ConsoleColor?)xForegroundComboBox.SelectedItem,
                                    (ConsoleColor?)xBackgroundComboBox.SelectedItem,
                                    mOnRemove));
            Close();
        }

        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
