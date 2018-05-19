// Copyright © 2018 Alex Leendertsen

using System.ComponentModel;
using System.Windows;
using System.Xml;
using Editor.Descriptors;
using Editor.Windows.Appenders.Properties;
using Editor.Windows.PropertyCommon;
using log4net.Appender;

namespace Editor.Windows.Appenders
{
    public class RollingFileAppenderWindow : FileAppenderWindow
    {
        private readonly DatePattern mDatePattern;
        private int mDatePatternIndex;
        private readonly StaticLogFileName mStaticLogFileName;
        private int mStaticLogFileNameIndex;

        public RollingFileAppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode)
            : base(owner, configXml, log4NetNode, appenderNode)
        {
            Title = "Rolling File Appender";
            Height = 700;
            MinHeight = 700;
            mDatePattern = new DatePattern(AppenderProperties);
            mStaticLogFileName = new StaticLogFileName(AppenderProperties);
        }

        protected override void AddOtherFileAppenderProperties()
        {
            RollingStyle rollingStyle = new RollingStyle(AppenderProperties);
            rollingStyle.PropertyChanged += RollingStyleOnPropertyChanged;

            AppenderProperties.Add(rollingStyle);

            mStaticLogFileNameIndex = AppenderProperties.Count;
            AddRemoveBasedOnMode(rollingStyle.SelectedMode, mStaticLogFileNameIndex, mStaticLogFileName);

            mDatePatternIndex = AppenderProperties.Count;
            AddRemoveBasedOnMode(rollingStyle.SelectedMode, mDatePatternIndex, mDatePattern);

            AppenderProperties.Add(new PreserveExtension(AppenderProperties));
            AppenderProperties.Add(new MaximumFileSize(AppenderProperties));
            AppenderProperties.Add(new MaxSizeRollBackups(AppenderProperties));
            AppenderProperties.Add(new CountDirection(AppenderProperties));
        }

        private void RollingStyleOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RollingStyle.SelectedMode))
            {
                RollingFileAppender.RollingMode selectedMode = ((RollingStyle)sender).SelectedMode;
                AddRemoveBasedOnMode(selectedMode, mDatePatternIndex, mDatePattern);
                AddRemoveBasedOnMode(selectedMode, mStaticLogFileNameIndex, mStaticLogFileName);
            }
        }

        private void AddRemoveBasedOnMode(RollingFileAppender.RollingMode selectedMode, int index, IProperty appenderProperty)
        {
            if (selectedMode == RollingFileAppender.RollingMode.Composite ||
                selectedMode == RollingFileAppender.RollingMode.Date)
            {
                if (!AppenderProperties.Contains(appenderProperty))
                {
                    AppenderProperties.Insert(index, appenderProperty);
                }
            }
            else
            {
                AppenderProperties.Remove(appenderProperty);
            }
        }

        protected override AppenderDescriptor Descriptor => AppenderDescriptor.RollingFile;
    }
}
