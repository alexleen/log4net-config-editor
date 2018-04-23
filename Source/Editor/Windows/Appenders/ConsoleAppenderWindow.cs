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
        public ConsoleAppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode, ChangeType changeType)
            : base(owner, configXml, log4NetNode, appenderNode, changeType)
        {
            Title = "Console Appender";
            xTargetRow.Height = AutoGridRowHeight;
        }

        protected override void LoadAppenderSpecificItems(XmlNode appenderNode)
        {
            string target = appenderNode.SelectSingleNode("target")?.Attributes?["value"]?.Value;

            if (target != null)
            {
                xTargetComboBox.SelectedItem = target;
            }
        }

        protected override AppenderDescriptor Descriptor => AppenderDescriptor.Console;

        protected override void SaveAppenderSpecificItems(XmlDocument configXml, XmlNode appenderNode)
        {
            const string consoleError = "Console.Error";

            //Target is "Console.Out" by default, so we only need a target element if "Console.Error"
            if ((string)xTargetComboBox.SelectedItem == consoleError)
            {
                configXml.CreateElementWithAttribute("target", "value", consoleError).AppentTo(appenderNode);
            }
        }
    }
}
