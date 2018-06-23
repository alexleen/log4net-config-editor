// Copyright © 2018 Alex Leendertsen

using System.ComponentModel;
using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Windows;
using static log4net.Appender.RollingFileAppender;

namespace Editor.Definitions.Appenders
{
    internal class RollingFileAppender : FileAppender
    {
        private readonly DatePattern mDatePattern;
        private int mDatePatternIndex;
        private readonly StaticLogFileName mStaticLogFileName;
        private int mStaticLogFileNameIndex;

        internal RollingFileAppender(IAppenderConfiguration appenderConfiguration, IMessageBoxService messageBoxService)
            : base(appenderConfiguration, messageBoxService)
        {
            mDatePattern = new DatePattern(Properties);
            mStaticLogFileName = new StaticLogFileName(Properties);
        }
        
        public override string Name => "Rolling File Appender";

        public override AppenderDescriptor Descriptor => AppenderDescriptor.RollingFile;

        protected override void AddAppenderSpecificProperties()
        {
            base.AddAppenderSpecificProperties();
            RollingStyle rollingStyle = new RollingStyle(Properties);
            rollingStyle.PropertyChanged += RollingStyleOnPropertyChanged;

            AddProperty(rollingStyle);

            mStaticLogFileNameIndex = Properties.Count;
            AddRemoveBasedOnMode(rollingStyle.SelectedMode, mStaticLogFileNameIndex, mStaticLogFileName);

            mDatePatternIndex = Properties.Count;
            AddRemoveBasedOnMode(rollingStyle.SelectedMode, mDatePatternIndex, mDatePattern);

            AddProperty(new PreserveExtension(Properties));
            AddProperty(new MaximumFileSize(Properties));
            AddProperty(new MaxSizeRollBackups(Properties));
            AddProperty(new CountDirection(Properties));
        }

        private void RollingStyleOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RollingStyle.SelectedMode))
            {
                RollingMode selectedMode = ((RollingStyle)sender).SelectedMode;
                AddRemoveBasedOnMode(selectedMode, mDatePatternIndex, mDatePattern);
                AddRemoveBasedOnMode(selectedMode, mStaticLogFileNameIndex, mStaticLogFileName);
            }
        }

        private void AddRemoveBasedOnMode(RollingMode selectedMode, int index, IProperty appenderProperty)
        {
            if (selectedMode == RollingMode.Composite ||
                selectedMode == RollingMode.Date)
            {
                if (!Properties.Contains(appenderProperty))
                {
                    AddProperty(index, appenderProperty);
                }
            }
            else
            {
                RemoveProperty(appenderProperty);
            }
        }
    }
}
