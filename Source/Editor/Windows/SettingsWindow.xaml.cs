// Copyright © 2019 Alex Leendertsen

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Editor.Descriptors;

namespace Editor.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private ObservableCollection<Tuple<string, string>> mMappings = new ObservableCollection<Tuple<string, string>>();

        public SettingsWindow()
        {
            InitializeComponent();

            xMapped.ItemsSource = AppenderDescriptor.DescriptorsByTypeNamespace.Keys;
            xMappings.ItemsSource = mMappings;
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            mMappings.Add(Tuple.Create(xCustom.Text, (string)xMapped.SelectedItem));

            xCustom.Text = string.Empty;
            xMapped.SelectedIndex = 0;
        }

        private void RemoveClick(object sender, RoutedEventArgs e)
        {
            mMappings.Remove((Tuple<string, string>)((Button)sender).Tag);
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
