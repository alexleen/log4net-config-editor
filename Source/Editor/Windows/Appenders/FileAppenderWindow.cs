// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Descriptors;
using Editor.Windows.Appenders.Properties;

namespace Editor.Windows.Appenders
{
    public class FileAppenderWindow : AppenderSkeletonWindow
    {
        public FileAppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode)
            : base(owner, configXml, log4NetNode, appenderNode)
        {
            Title = "File Appender";
        }

        protected override void AddAppenderSpecificProperties()
        {
            AppenderProperties.Add(new File(AppenderProperties, this));
            AppenderProperties.Add(new LockingModel(AppenderProperties));
        }

        protected override AppenderDescriptor Descriptor => AppenderDescriptor.File;
    }
}
