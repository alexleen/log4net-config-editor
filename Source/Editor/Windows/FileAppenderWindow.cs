// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;

namespace Editor.Windows
{
    public class FileAppenderWindow : AppenderWindow
    {
        public FileAppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode)
            : base(owner, configXml, log4NetNode, appenderNode)
        {
            xTargetRow.Height = new GridLength(0);
        }
    }
}
