// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Descriptors;
using Editor.HistoryManager;
using Editor.Windows.Appenders.Properties;

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
            Name nameProperty = new Name(AppenderProperties, Log4NetNode, OriginalAppenderNode);
            AppenderProperties.Add(nameProperty);
            AppenderProperties.Add(new Target(AppenderProperties));
            AppenderProperties.Add(new Layout(AppenderProperties, new HistoryManager.HistoryManager("HistoricalPatterns", new SettingManager<string>())));
            AppenderProperties.Add(new Properties.Filters(this, ConfigXml, NewAppenderNode, AppenderProperties));
            AppenderProperties.Add(new Refs(Log4NetNode, nameProperty, AppenderProperties));
        }

        protected override AppenderDescriptor Descriptor => AppenderDescriptor.Console;
    }
}
