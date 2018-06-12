// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Descriptors;
using Editor.Windows.Appenders.Properties;

namespace Editor.Windows.Appenders
{
    public class ForwardingAppenderWindow : AppenderSkeletonWindow
    {
        public ForwardingAppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode)
            : base(owner, configXml, log4NetNode, appenderNode)
        {
            Title = "Forwarding Appender";
        }

        protected override void AddAppenderSpecificProperties()
        {
            AppenderProperties.Add(new OutgoingRefs(Log4NetNode, AppenderProperties, OriginalAppenderNode));
        }

        protected override AppenderDescriptor Descriptor => AppenderDescriptor.Forwarding;
    }
}
