// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Descriptors;
using Editor.Enums;
using Editor.Utilities;

namespace Editor.Windows.Appenders
{
    public class ConsoleAppenderWindow : AppenderWindow
    {
        private const string ConsoleOut = "Console.Out";
        private const string ConsoleError = "Console.Error";

        public ConsoleAppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode, ChangeType changeType)
            : base(owner, configXml, log4NetNode, appenderNode, changeType)
        {
            Title = "Console Appender";
            xTargetRow.Height = AutoGridRowHeight;
            xTargetComboBox.ItemsSource = new[] { ConsoleOut, ConsoleError };
        }

        protected override void LoadAppenderSpecificItems(XmlNode appenderNode)
        {
            string target = appenderNode["target"]?.Attributes["value"]?.Value;
            xTargetComboBox.SelectedItem = target ?? ConsoleOut;
        }

        protected override AppenderDescriptor Descriptor => AppenderDescriptor.Console;

        protected override void SaveAppenderSpecificItems(XmlDocument configXml, XmlNode appenderNode)
        {
            //Target is "Console.Out" by default, so we only need a target element if "Console.Error"
            if ((string)xTargetComboBox.SelectedItem == ConsoleError)
            {
                configXml.CreateElementWithAttribute("target", "value", ConsoleError).AppendTo(appenderNode);
            }
        }
    }
}
