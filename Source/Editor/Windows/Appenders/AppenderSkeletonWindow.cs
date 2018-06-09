// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.HistoryManager;
using Editor.Windows.Appenders.Properties;

namespace Editor.Windows.Appenders
{
    public abstract class AppenderSkeletonWindow : AppenderWindow
    {
        protected AppenderSkeletonWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode)
            : base(owner, configXml, log4NetNode, appenderNode)
        {
        }

        protected sealed override void AddAppropriateProperties()
        {
            Name nameProperty = new Name(AppenderProperties, Log4NetNode, OriginalAppenderNode);
            AppenderProperties.Add(nameProperty);
            AppenderProperties.Add(new Threshold(AppenderProperties));

            AddAppenderSpecificProperties();

            AppenderProperties.Add(new Layout(AppenderProperties, new HistoryManager.HistoryManager("HistoricalPatterns", new SettingManager<string>())));
            AppenderProperties.Add(new Properties.Filters(ConfigXml, NewAppenderNode, AppenderProperties, this));
            AppenderProperties.Add(new IncomingRefs(Log4NetNode, nameProperty, AppenderProperties, OriginalAppenderNode));
        }

        protected virtual void AddAppenderSpecificProperties()
        {
        }
    }
}
