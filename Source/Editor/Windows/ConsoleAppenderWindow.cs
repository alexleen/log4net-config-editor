// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;

namespace Editor.Windows
{
    public class ConsoleAppenderWindow : AppenderWindow
    {
        public ConsoleAppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode)
            : base(owner, configXml, log4NetNode, appenderNode)
        {
            xFileRow.Height = new GridLength(0);
        }
    }
}
