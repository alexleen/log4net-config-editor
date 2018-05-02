// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Descriptors;
using Editor.Enums;
using Editor.Windows.Appenders.Properties;

namespace Editor.Windows.Appenders
{
    public class AsyncAppenderWindow : AppenderWindow
    {
        public AsyncAppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode, ChangeType changeType)
            : base(owner, configXml, log4NetNode, appenderNode, changeType)
        {
            Title = "Async Appender";
        }

        protected override void AddAppropriateProperties()
        {
            Name nameProperty = new Name(AppenderProperties, AppenderNode);
            AppenderProperties.Add(nameProperty);
            AppenderProperties.Add(new Fix(AppenderProperties, AppenderNode));
            AppenderProperties.Add(new BufferSize(AppenderProperties, AppenderNode));
            AppenderProperties.Add(new Refs(Log4NetNode, nameProperty, AppenderProperties, AppenderNode));
        }

        protected override AppenderDescriptor Descriptor => AppenderDescriptor.Async;
    }
}
