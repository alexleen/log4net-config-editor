// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;

namespace Editor.Windows.Appenders
{
    public class ConsoleAppenderWindow : AppenderWindow
    {
        public ConsoleAppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode)
            : base(owner, configXml, log4NetNode, appenderNode)
        {
            Title = "Console Appender";
            xFileRow.Height = new GridLength(0);
        }

        protected override void LoadAppenderSpecificItems(XmlNode appenderNode)
        {
            string target = appenderNode.SelectSingleNode("target")?.Attributes?["value"]?.Value;

            if (target != null)
            {
                xTargetComboBox.SelectedItem = target;
            }
        }

        protected override void SaveAppenderSpecificItems(XmlDocument configXml, XmlNode appenderNode)
        {
            const string consoleError = "Console.Error";

            //Target is "Console.Out" by default, so we only need a target element if "Console.Error"
            if ((string)xTargetComboBox.SelectedItem == consoleError)
            {
                XmlAttribute targetValueAttr = configXml.CreateAttribute("value");
                targetValueAttr.Value = consoleError;

                XmlElement targetElement = configXml.CreateElement("target");
                targetElement.Attributes.Append(targetValueAttr);

                appenderNode.AppendChild(targetElement);
            }
        }
    }
}
