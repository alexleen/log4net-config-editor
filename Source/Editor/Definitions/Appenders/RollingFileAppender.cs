// Copyright © 2020 Alex Leendertsen

using System.Collections.Generic;
using System.ComponentModel;
using Editor.ConfigProperties;
using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Utilities;
using static log4net.Appender.RollingFileAppender;

namespace Editor.Definitions.Appenders
{
    internal class RollingFileAppender : FileAppender
    {
        private readonly CountDirection mCountDirection;
        private readonly ISet<RollingMode> mCountDirectionModes = new HashSet<RollingMode> { RollingMode.Composite, RollingMode.Once, RollingMode.Size };

        private readonly DatePattern mDatePattern;
        private readonly StringValueProperty mDateTimeStrategy;
        private readonly ISet<RollingMode> mDateModes = new HashSet<RollingMode> { RollingMode.Composite, RollingMode.Date };

        private readonly MaximumFileSize mMaximumFileSize;
        private readonly ISet<RollingMode> mMaximumFileSizeModes = new HashSet<RollingMode> { RollingMode.Composite, RollingMode.Size };

        private int mCountDirectionIndex;
        private int mDatePatternIndex;
        private int mMaximumFileSizeIndex;
        private int mDateTimeStrategyIndex;

        internal RollingFileAppender(IElementConfiguration configuration)
            : base(configuration)
        {
            mDatePattern = new DatePattern();
            mMaximumFileSize = new MaximumFileSize();
            mCountDirection = new CountDirection();
            mDateTimeStrategy = new StringValueProperty("Date Time Strategy:", "dateTimeStrategy", Log4NetXmlConstants.Type)
            {
                ToolTip = "Sets the strategy for determining the current date and time.\n" +
                          "The default implementation is to use LocalDateTime (log4net.Appender.RollingFileAppender+LocalDateTime,log4net) which internally calls through to DateTime.Now.\n" +
                          "DateTime.UtcNow may be used on frameworks newer than .NET 1.0 by specifying UniversalDateTime (log4net.Appender.RollingFileAppender+UniversalDateTime,log4net).\n" +
                          "A custom implementation that implements IDateTime can be specified here as well. Leave blank to use LocalDateTime."
            };
        }

        public override string Name => "Rolling File Appender";

        public override AppenderDescriptor Descriptor => AppenderDescriptor.RollingFile;

        protected override void AddAppenderSpecificProperties()
        {
            base.AddAppenderSpecificProperties();
            RollingStyle rollingStyle = new RollingStyle();
            rollingStyle.PropertyChanged += RollingStyleOnPropertyChanged;

            AddProperty(rollingStyle);

            mDateTimeStrategyIndex = Properties.Count;
            AddRemoveBasedOnMode(rollingStyle.SelectedMode, mDateModes, mDateTimeStrategyIndex, mDateTimeStrategy);

            AddProperty(new BooleanPropertyBase("Static Log File Name:", "staticLogFileName", true));

            AddProperty(new BooleanPropertyBase("Preserve Extension:", "preserveLogFileNameExtension", false));

            mDatePatternIndex = Properties.Count;
            AddRemoveBasedOnMode(rollingStyle.SelectedMode, mDateModes, mDatePatternIndex, mDatePattern);

            mMaximumFileSizeIndex = Properties.Count;
            AddRemoveBasedOnMode(rollingStyle.SelectedMode, mMaximumFileSizeModes, mMaximumFileSizeIndex, mMaximumFileSize);

            AddProperty(new MaxSizeRollBackups());

            mCountDirectionIndex = Properties.Count;
            AddRemoveBasedOnMode(rollingStyle.SelectedMode, mCountDirectionModes, mCountDirectionIndex, mCountDirection);
        }

        private void RollingStyleOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RollingStyle.SelectedMode))
            {
                RollingMode selectedMode = ((RollingStyle)sender).SelectedMode;
                AddRemoveBasedOnMode(selectedMode, mDateModes, mDateTimeStrategyIndex, mDateTimeStrategy);
                AddRemoveBasedOnMode(selectedMode, mDateModes, mDatePatternIndex, mDatePattern);
                AddRemoveBasedOnMode(selectedMode, mMaximumFileSizeModes, mMaximumFileSizeIndex, mMaximumFileSize);
                AddRemoveBasedOnMode(selectedMode, mCountDirectionModes, mCountDirectionIndex, mCountDirection);
            }
        }

        private void AddRemoveBasedOnMode(RollingMode selectedMode, ISet<RollingMode> acceptableModes, int index, IProperty appenderProperty)
        {
            if (acceptableModes.Contains(selectedMode))
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
