// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Descriptors;
using Editor.Windows.Appenders.Properties;
using Editor.Windows.Appenders.Properties.PatternManager;

namespace Editor.Windows.Appenders
{
    public class ConsoleAppenderWindow : AppenderWindow
    {
        public ConsoleAppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode)
            : base(owner, configXml, log4NetNode, appenderNode)
        {
            Title = "Console Appender";
        }

        protected override void AddAppropriateProperties()
        {
            Name nameProperty = new Name(AppenderProperties);
            AppenderProperties.Add(nameProperty);
            AppenderProperties.Add(new Target(AppenderProperties));

            AddOtherConsoleAppenderProperties();

            AppenderProperties.Add(new Layout(AppenderProperties, new HistoricalPatternManager()));
            AppenderProperties.Add(new Properties.Filters(this, ConfigXml, NewAppenderNode, AppenderProperties));
            AppenderProperties.Add(new Refs(Log4NetNode, nameProperty, AppenderProperties));
        }

        protected virtual void AddOtherConsoleAppenderProperties()
        {
        }

        protected override AppenderDescriptor Descriptor => AppenderDescriptor.Console;
    }
}
