// Copyright © 2018 Alex Leendertsen

using System;
using System.Linq;
using System.Windows;
using System.Xml;
using Editor.Descriptors;
using Editor.Enums;
using Editor.Utilities;
using log4net.Core;

namespace Editor.Windows.Appenders
{
    public class AsyncAppenderWindow : AppenderWindow
    {
        private const string DefaultBufferSize = "1000";

        public AsyncAppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode, ChangeType changeType)
            : base(owner, configXml, log4NetNode, appenderNode, changeType)
        {
            Title = "Asyn Appender";
            xFixRow.Height = AutoGridRowHeight;
            xBufferSizeRow.Height = AutoGridRowHeight;
            xLayoutRow.Height = new GridLength(0);
            xFiltersRow.Height = new GridLength(0);

            xFixComboBox.ItemsSource = Enum.GetValues(typeof(FixFlags)).Cast<FixFlags>();
        }

        protected override void LoadAppenderSpecificItems(XmlNode appenderNode)
        {
            string fixValue = appenderNode["Fix"]?.Attributes["value"].Value;

            if (int.TryParse(fixValue, out int fixValueInt) && Enum.IsDefined(typeof(FixFlags), fixValueInt))
            {
                xFixComboBox.SelectedItem = (FixFlags)fixValueInt;
            }
            else
            {
                xFilterComboBox.SelectedIndex = 0;
            }

            string bufferSizeStr = appenderNode["bufferSize"]?.Attributes["value"].Value;

            if (int.TryParse(bufferSizeStr, out int _))
            {
                xBufferSizeTextBox.Text = bufferSizeStr;
            }
            else
            {
                xBufferSizeTextBox.Text = DefaultBufferSize;
            }
        }

        protected override AppenderDescriptor Descriptor => AppenderDescriptor.Async;

        protected override bool TryValidateInputs()
        {
            if (!int.TryParse(xBufferSizeTextBox.Text, out int _))
            {
                MessageBox.Show(this, "Buffer size must be a valid integer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return base.TryValidateInputs();
        }

        protected override void SaveAppenderSpecificItems(XmlDocument configXml, XmlNode appenderNode)
        {
            configXml.CreateElementWithAttribute("Fix", "value", ((int)xFixComboBox.SelectedItem).ToString()).AppendTo(appenderNode);

            if (xBufferSizeTextBox.Text != DefaultBufferSize)
            {
                configXml.CreateElementWithAttribute("bufferSize", "value", xBufferSizeTextBox.Text).AppendTo(appenderNode);
            }
        }

        protected override void SaveLayout(XmlNode appenderNode)
        {
            //We don't want to dave a layout for an async appender
        }
    }
}
