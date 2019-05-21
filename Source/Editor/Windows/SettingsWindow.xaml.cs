// Copyright © 2019 Alex Leendertsen

using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;
using Editor.Descriptors;
using Editor.Models;
using Editor.Properties;

namespace Editor.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly ObservableCollection<AppenderMapping> mMappings = new ObservableCollection<AppenderMapping>();

        public SettingsWindow()
        {
            InitializeComponent();

            xMapped.ItemsSource = AppenderDescriptor.DescriptorsByTypeNamespace.Keys;
            xMappings.ItemsSource = mMappings;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Settings.Default.AppenderMappings))
            {
                return;
            }

            AppenderMapping[] mappings;
            using (StringReader strReader = new StringReader(Settings.Default.AppenderMappings))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AppenderMapping[]));
                mappings = (AppenderMapping[])serializer.Deserialize(strReader);
            }

            foreach (AppenderMapping m in mappings)
            {
                mMappings.Add(m);
            }
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            mMappings.Add(new AppenderMapping { Custom = xCustom.Text, Mapped = (string)xMapped.SelectedItem });

            xCustom.Text = string.Empty;
            xMapped.SelectedIndex = 0;
        }

        private void RemoveClick(object sender, RoutedEventArgs e)
        {
            mMappings.Remove((AppenderMapping)((Button)sender).Tag);
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            //TODO unique mappings
            string xml;
            using (StringWriter textWriter = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AppenderMapping[]));
                serializer.Serialize(textWriter, mMappings.ToArray());

                xml = textWriter.ToString();
            }

            Settings.Default.AppenderMappings = xml;
            Settings.Default.Save();

            Close();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
