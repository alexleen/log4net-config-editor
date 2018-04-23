// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Descriptors;
using Editor.Enums;
using Editor.Utilities;

namespace Editor.Windows.Appenders
{
    public class EventLogAppenderWindow : AppenderWindow
    {
        private const string LogName = "LogName";
        private const string ApplicationName = "ApplicationName";

        public EventLogAppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode, ChangeType changeType)
            : base(owner, configXml, log4NetNode, appenderNode, changeType)
        {
            Title = "Event Log Appender";
            xLogNameRow.Height = AutoGridRowHeight;
            xApplicationNameRow.Height = AutoGridRowHeight;
        }

        protected override AppenderDescriptor Descriptor => AppenderDescriptor.EventLog;

        protected override bool TryValidateInputs()
        {
            if (string.IsNullOrEmpty(xLogNameTextBox.Text))
            {
                MessageBox.Show(this, "A log name must be assigned to this appender.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrEmpty(xApplicationNameTextBox.Text))
            {
                MessageBox.Show(this, "An application name must be assigned to this appender.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return base.TryValidateInputs();
        }

        protected override void LoadAppenderSpecificItems(XmlNode appenderNode)
        {
            XmlNode logNameNode = appenderNode.SelectSingleNode($"param[@name='{LogName}']");
            ReplaceIfNotNullOrEmpty(logNameNode?.Attributes?["value"]?.Value, xLogNameTextBox);

            XmlNode applicationNameNode = appenderNode.SelectSingleNode($"param[@name='{ApplicationName}']");
            ReplaceIfNotNullOrEmpty(applicationNameNode?.Attributes?["value"]?.Value, xApplicationNameTextBox);
        }

        protected override void SaveAppenderSpecificItems(XmlDocument configXml, XmlNode appenderNode)
        {
            configXml.CreateElementWithAttributes("param",
                                                  new[]
                                                  {
                                                      (Name: "name", Value: LogName),
                                                      (Name: "value", Value: xLogNameTextBox.Text)
                                                  })
                     .AppendTo(appenderNode);

            configXml.CreateElementWithAttributes("param",
                                                  new[]
                                                  {
                                                      (Name: "name", Value: ApplicationName),
                                                      (Name: "value", Value: xApplicationNameTextBox.Text)
                                                  })
                     .AppendTo(appenderNode);
        }
    }
}
