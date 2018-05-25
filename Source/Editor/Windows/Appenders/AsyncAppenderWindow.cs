// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Descriptors;
using Editor.Windows.Appenders.Properties;

namespace Editor.Windows.Appenders
{
    public class AsyncAppenderWindow : AppenderWindow
    {
        public AsyncAppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode)
            : base(owner, configXml, log4NetNode, appenderNode)
        {
            Title = "Async Appender";
        }

        protected override void AddAppropriateProperties()
        {
            Name nameProperty = new Name(AppenderProperties, Log4NetNode);
            AppenderProperties.Add(nameProperty);
            AppenderProperties.Add(new Fix(AppenderProperties));
            AppenderProperties.Add(new BufferSize(AppenderProperties));
            AppenderProperties.Add(new Refs(Log4NetNode, nameProperty, AppenderProperties));
        }

        protected override AppenderDescriptor Descriptor => AppenderDescriptor.Async;
    }
}
