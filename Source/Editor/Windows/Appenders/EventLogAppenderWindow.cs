// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Descriptors;
using Editor.Windows.Appenders.Properties;

namespace Editor.Windows.Appenders
{
    public class EventLogAppenderWindow : AppenderSkeletonWindow
    {
        public EventLogAppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode)
            : base(owner, configXml, log4NetNode, appenderNode)
        {
            Title = "Event Log Appender";
        }

        protected override void AddAppenderSpecificProperties()
        {
            AppenderProperties.Add(new LogName(AppenderProperties));
            AppenderProperties.Add(new ApplicationName(AppenderProperties));
        }

        protected override AppenderDescriptor Descriptor => AppenderDescriptor.EventLog;
    }
}
