// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Descriptors;
using Editor.Windows.Appenders.Properties;

namespace Editor.Windows.Appenders
{
    public class AsyncAppenderWindow : ForwardingAppenderWindow
    {
        public AsyncAppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode)
            : base(owner, configXml, log4NetNode, appenderNode)
        {
            Title = "Async Appender";
        }

        protected override void AddAppenderSpecificProperties()
        {
            AppenderProperties.Add(new Fix(AppenderProperties) { SelectedPreset = Fix.PartialPreset });
            AppenderProperties.Add(new BufferSize(AppenderProperties));
            base.AddAppenderSpecificProperties();
        }

        protected override AppenderDescriptor Descriptor => AppenderDescriptor.Async;
    }
}
