// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Descriptors;
using Editor.Enums;
using Editor.Windows.Appenders.Properties;

namespace Editor.Windows.Appenders
{
    public class FileAppenderWindow : AppenderWindow
    {
        public FileAppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode, ChangeType changeType)
            : base(owner, configXml, log4NetNode, appenderNode, changeType)
        {
            Title = "File Appender";
        }

        protected override void AddAppropriateProperties()
        {
            Name nameProperty = new Name(AppenderProperties);
            AppenderProperties.Add(nameProperty);
            AppenderProperties.Add(new File(AppenderProperties));
            AppenderProperties.Add(new Layout(AppenderProperties));
            AppenderProperties.Add(new Properties.Filters(this, ConfigXml, AppenderNode, AppenderProperties));
            AppenderProperties.Add(new Refs(Log4NetNode, nameProperty, AppenderProperties));
        }

        protected override AppenderDescriptor Descriptor => AppenderDescriptor.File;
    }
}
