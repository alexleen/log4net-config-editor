// Copyright © 2019 Alex Leendertsen

using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;
using Editor.Descriptors;

namespace Editor.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private ObservableCollection<Mapping> mMappings = new ObservableCollection<Mapping>();

        public SettingsWindow()
        {
            InitializeComponent();

            xMapped.ItemsSource = AppenderDescriptor.DescriptorsByTypeNamespace.Keys;
            xMappings.ItemsSource = mMappings;
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            mMappings.Add(new Mapping { Custom = xCustom.Text, Mapped = (string)xMapped.SelectedItem });

            xCustom.Text = string.Empty;
            xMapped.SelectedIndex = 0;
        }

        private void RemoveClick(object sender, RoutedEventArgs e)
        {
            mMappings.Remove((Mapping)((Button)sender).Tag);
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Mapping[]));
            using (StringWriter textWriter = new StringWriter())
            {
                serializer.Serialize(textWriter, mMappings.ToArray());
                string xml = textWriter.ToString();
            }

            Close();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    public class Mapping
    {
        public string Custom { get; set; }

        public string Mapped { get; set; }
    }
}
