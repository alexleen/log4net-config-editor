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
        private readonly XmlDocument mConfigXml;
        private readonly XmlNode mLog4NetNode;
        private readonly XmlNode mAppenderNode;

        public AddFileAppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode)
        {
            InitializeComponent();
            Owner = owner;
            mConfigXml = configXml;
            mLog4NetNode = log4NetNode;
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

        private void SaveOnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(xNameTextBox.Text))
            {
                MessageBox.Show(this, "A name must be assigned to this appender.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            if (string.IsNullOrEmpty(xFileTextBox.Text))
            {
                MessageBox.Show(this, "A file must be assigned to this appender.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(xPatternTextBox.Text))
            {
                MessageBox.Show(this, "A pattern must be assigned to this appender.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (mAppenderNode == null) //New appender
            {
                XmlAttribute nameAttribute = mConfigXml.CreateAttribute("name");
                nameAttribute.Value = xNameTextBox.Text;

                XmlAttribute appenderTypeAttribute = mConfigXml.CreateAttribute("type");
                appenderTypeAttribute.Value = AppenderDescriptor.File.TypeNamespace;

                XmlNode appenderNode = mConfigXml.CreateNode(XmlNodeType.Element, "appender", null);
                appenderNode.Attributes.Append(nameAttribute);
                appenderNode.Attributes.Append(appenderTypeAttribute);

                XmlAttribute fileValueAttr = mConfigXml.CreateAttribute("value");
                fileValueAttr.Value = xFileTextBox.Text;
                XmlElement fileElement = mConfigXml.CreateElement("file");
                fileElement.Attributes.Append(fileValueAttr);
                appenderNode.AppendChild(fileElement);

                if (xOverwriteRb.IsChecked.HasValue && xOverwriteRb.IsChecked.Value)
                {
                    XmlAttribute appendToFileValueAttr = mConfigXml.CreateAttribute("value");
                    appendToFileValueAttr.Value = "false";
                    XmlElement appendToFileElement = mConfigXml.CreateElement("appendToFile");
                    appendToFileElement.Attributes.Append(appendToFileValueAttr);
                    appenderNode.AppendChild(appendToFileElement);
                }

                XmlNode layoutNode = mConfigXml.CreateElement("layout");

                XmlAttribute layoutTypeAttribute = mConfigXml.CreateAttribute("type");
                layoutTypeAttribute.Value = ((LayoutDescriptor)xLayoutComboBox.SelectionBoxItem).TypeNamespace;
                layoutNode.Attributes.Append(layoutTypeAttribute);

                XmlAttribute convPatternValueAttr = mConfigXml.CreateAttribute("value");
                convPatternValueAttr.Value = xPatternTextBox.Text;

                XmlElement convPatternElement = mConfigXml.CreateElement("conversionPattern");
                convPatternElement.Attributes.Append(convPatternValueAttr);
                layoutNode.AppendChild(convPatternElement);

                appenderNode.AppendChild(layoutNode);

                mLog4NetNode.AppendChild(appenderNode);
            }

            Close();
        }

        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
