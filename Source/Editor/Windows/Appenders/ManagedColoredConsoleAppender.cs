// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;

namespace Editor.Windows.Appenders
{
    public class ManagedColoredConsoleAppender : ConsoleAppenderWindow
    {
        public ManagedColoredConsoleAppender(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode)
            : base(owner, configXml, log4NetNode, appenderNode)
        {
        }

        protected override void AddOtherConsoleAppenderProperties()
        {
            
        }
    }
}
