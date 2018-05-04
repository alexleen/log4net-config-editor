// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Descriptors;
using Editor.Windows.Appenders.Properties;

namespace Editor.Windows.Appenders
{
    public class RollingFileAppenderWindow : FileAppenderWindow
    {
        public RollingFileAppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode)
            : base(owner, configXml, log4NetNode, appenderNode)
        {
            Title = "Rolling File Appender";
            Height = 600;
        }

        protected override void AddOtherFileAppenderProperties()
        {
            AppenderProperties.Add(new StaticLogFileName(AppenderProperties));
            AppenderProperties.Add(new RollingStyle(AppenderProperties));
            AppenderProperties.Add(new DatePattern(AppenderProperties));
            AppenderProperties.Add(new MaximumFileSize(AppenderProperties));
            AppenderProperties.Add(new MaxSizeRollBackups(AppenderProperties));
        }

        protected override AppenderDescriptor Descriptor => AppenderDescriptor.RollingFile;
    }
}
