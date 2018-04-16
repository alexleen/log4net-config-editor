// Copyright © 2018 Alex Leendertsen

using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using System.Xml;
using Editor.Descriptors;

namespace Editor.Windows
{
    /// <summary>
    /// Interaction logic for AddFileAppenderWindow.xaml
    /// </summary>
    public partial class AddFileAppenderWindow : Window
    {
        private readonly XmlNode mAppenderNode;

        public AddFileAppenderWindow(Window owner, XmlNode appenderNode)
        {
            InitializeComponent();
            Owner = owner;
            mAppenderNode = appenderNode;
            xLayoutComboBox.ItemsSource = new[]
            {
                LayoutDescriptor.Simple,
                LayoutDescriptor.Pattern
            };

            Load();
        }

        private void Load()
        {
            xNameTextBox.Text = mAppenderNode?.Attributes?["name"]?.Value;
            xFileTextBox.Text = mAppenderNode?.SelectSingleNode("file")?.Attributes?["value"]?.Value;
            string appendToFile = mAppenderNode?.SelectSingleNode("appendToFile")?.Attributes?["value"]?.Value;
            xOverwriteRb.IsChecked = appendToFile == "false";

            string layoutType = mAppenderNode?.SelectSingleNode("layout")?.Attributes?["type"]?.Value;
            if (LayoutDescriptor.TryFindByTypeNamespace(layoutType, out LayoutDescriptor descriptor))
            {
                xLayoutComboBox.SelectedItem = descriptor;
            }

            string conversionPattern = mAppenderNode?.SelectSingleNode("//conversionPattern")?.Attributes?["value"]?.Value;
            xPatternTextBox.Text = conversionPattern;
        }

        private void HyperlinkOnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
