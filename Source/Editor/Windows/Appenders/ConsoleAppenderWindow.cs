// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Descriptors;
using Editor.Enums;
using Editor.Windows.Appenders.Properties;

namespace Editor.Windows.Appenders
{
    public class ConsoleAppenderWindow : AppenderWindow
    {
        public ConsoleAppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode, ChangeType changeType)
            : base(owner, configXml, log4NetNode, appenderNode, changeType)
        {
            Title = "Console Appender";
        }

        protected override void AddAppropriateProperties()
        {
            Name nameProperty = new Name(AppenderProperties, GridLength.Auto);
            AppenderProperties.Add(nameProperty);
            AppenderProperties.Add(new Target(AppenderProperties, GridLength.Auto));
            AppenderProperties.Add(new Layout(AppenderProperties, GridLength.Auto));
            AppenderProperties.Add(new Properties.Filters(this, ConfigXml, AppenderNode, AppenderProperties, new GridLength(1, GridUnitType.Star)));
            AppenderProperties.Add(new Refs(Log4NetNode, nameProperty, AppenderProperties, new GridLength(1, GridUnitType.Star)));
        }

        protected override AppenderDescriptor Descriptor => AppenderDescriptor.Console;
    }
}
