// Copyright © 2018 Alex Leendertsen

using System.ComponentModel;
using System.Windows;
using System.Xml;
using Editor.Descriptors;
using Editor.Windows.Appenders.Properties;
using log4net.Appender;

namespace Editor.Windows.Appenders
{
    public class RollingFileAppenderWindow : FileAppenderWindow
    {
        private readonly DatePattern mDatePattern;
        private int mDatePatternIndex;

        public RollingFileAppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode)
            : base(owner, configXml, log4NetNode, appenderNode)
        {
            Title = "Rolling File Appender";
            Height = 650;
            mDatePattern = new DatePattern(AppenderProperties);
        }

        protected override void AddOtherFileAppenderProperties()
        {
            RollingStyle rollingStyle = new RollingStyle(AppenderProperties);
            rollingStyle.PropertyChanged += RollingStyleOnPropertyChanged;

            AppenderProperties.Add(new StaticLogFileName(AppenderProperties));
            AppenderProperties.Add(rollingStyle);
            mDatePatternIndex = AppenderProperties.Count;
            AddRemoveDatePatternBasedOnMode(rollingStyle.SelectedMode);
            AppenderProperties.Add(new MaximumFileSize(AppenderProperties));
            AppenderProperties.Add(new MaxSizeRollBackups(AppenderProperties));
        }

        private void RollingStyleOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RollingStyle.SelectedMode))
            {
                AddRemoveDatePatternBasedOnMode(((RollingStyle)sender).SelectedMode);
            }
        }

        private void AddRemoveDatePatternBasedOnMode(RollingFileAppender.RollingMode selectedMode)
        {
            if (selectedMode == RollingFileAppender.RollingMode.Composite ||
                selectedMode == RollingFileAppender.RollingMode.Date)
            {
                if (!AppenderProperties.Contains(mDatePattern))
                {
                    AppenderProperties.Insert(mDatePatternIndex, mDatePattern);
                }
            }
            else
            {
                AppenderProperties.Remove(mDatePattern);
            }
        }

        protected override AppenderDescriptor Descriptor => AppenderDescriptor.RollingFile;
    }
}
