// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Descriptors;
using Editor.Enums;
using Editor.Utilities;

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
            configXml.CreateElementWithAttribute("file", "value", xFileTextBox.Text).AppendTo(appenderNode);

            //"appendToFile" is true by default, so we only need to change it to false if Overwrite is true
            if (xOverwriteRb.IsChecked.HasValue && xOverwriteRb.IsChecked.Value)
            {
                configXml.CreateElementWithAttribute("appendToFile", "value", "false").AppendTo(appenderNode);
            }
        }
    }
}
