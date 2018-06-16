// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Descriptors;
using Editor.Windows.Appenders.Properties;

namespace Editor.Windows.Appenders
{
    public class ManagedColoredConsoleAppender : ConsoleAppenderWindow
    {
        public ManagedColoredConsoleAppender(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode)
            : base(owner, configXml, log4NetNode, appenderNode)
        {
            Title = "Managed Color Console Appender";
        }

        protected override void AddAppenderSpecificProperties()
        {
            base.AddAppenderSpecificProperties();
            AppenderProperties.Add(new Mapping(this, AppenderProperties));
        }

        protected override AppenderDescriptor Descriptor => AppenderDescriptor.ManagedColor;
    }
}
