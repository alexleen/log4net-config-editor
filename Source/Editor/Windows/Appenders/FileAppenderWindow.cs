// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Descriptors;
using Editor.Enums;

namespace Editor.Windows.Appenders
{
    public class FileAppenderWindow : AppenderWindow
    {
        public FileAppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode, ChangeType changeType)
            : base(owner, configXml, log4NetNode, appenderNode, changeType)
        {
            Title = "File Appender";
            xFileRow.Height = AutoGridRowHeight;
        }

        protected override void LoadAppenderSpecificItems(XmlNode appenderNode)
        {
            ReplaceIfNotNullOrEmpty(appenderNode.SelectSingleNode("file")?.Attributes?["value"]?.Value, xFileTextBox);

            string appendToFile = appenderNode.SelectSingleNode("appendToFile")?.Attributes?["value"]?.Value;
            if (!string.IsNullOrEmpty(appendToFile))
            {
                xOverwriteRb.IsChecked = appendToFile == "false";
            }
        }

        protected override AppenderDescriptor Descriptor => AppenderDescriptor.File;

        protected override bool TryValidateInputs()
        {
            if (string.IsNullOrEmpty(xFileTextBox.Text))
            {
                MessageBox.Show(this, "A file must be assigned to this appender.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return base.TryValidateInputs();
        }

        protected override void SaveAppenderSpecificItems(XmlDocument configXml, XmlNode appenderNode)
        {
            XmlAttribute fileValueAttr = configXml.CreateAttribute("value");
            fileValueAttr.Value = xFileTextBox.Text;
            XmlElement fileElement = configXml.CreateElement("file");
            fileElement.Attributes.Append(fileValueAttr);
            appenderNode.AppendChild(fileElement);

            //"appendToFile" is true by default, so we only need to change it to false if Overwrite is true
            if (xOverwriteRb.IsChecked.HasValue && xOverwriteRb.IsChecked.Value)
            {
                XmlAttribute appendToFileValueAttr = configXml.CreateAttribute("value");
                appendToFileValueAttr.Value = "false";
                XmlElement appendToFileElement = configXml.CreateElement("appendToFile");
                appendToFileElement.Attributes.Append(appendToFileValueAttr);
                appenderNode.AppendChild(appendToFileElement);
            }
        }
    }
}
