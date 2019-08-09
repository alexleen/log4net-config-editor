// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.ComponentModel;
using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;
using static log4net.Appender.RollingFileAppender;

namespace Editor.Definitions.Appenders
{
    internal class RollingFileAppender : FileAppender
    {
        private readonly DatePattern mDatePattern;
        private int mDatePatternIndex;
        private readonly ISet<RollingMode> mDatePatternModes = new HashSet<RollingMode> { RollingMode.Composite, RollingMode.Date };

        private readonly MaximumFileSize mMaximumFileSize;
        private int mMaximumFileSizeIndex;
        private readonly ISet<RollingMode> mMaximumFileSizeModes = new HashSet<RollingMode> { RollingMode.Composite, RollingMode.Size };

        private readonly CountDirection mCountDirection;
        private int mCountDirectionIndex;
        private readonly ISet<RollingMode> mCountDirectionModes = new HashSet<RollingMode> { RollingMode.Composite, RollingMode.Once, RollingMode.Size };

        internal RollingFileAppender(IElementConfiguration configuration)
            : base(configuration)
        {
            mDatePattern = new DatePattern(Properties);
            mMaximumFileSize = new MaximumFileSize(Properties);
            mCountDirection = new CountDirection(Properties);
        }

        public override string Name => "Rolling File Appender";

        public override AppenderDescriptor Descriptor => AppenderDescriptor.RollingFile;

        protected override void AddAppenderSpecificProperties()
        {
            base.AddAppenderSpecificProperties();
            RollingStyle rollingStyle = new RollingStyle(Properties);
            rollingStyle.PropertyChanged += RollingStyleOnPropertyChanged;

            AddProperty(rollingStyle);

            AddProperty(new StaticLogFileName(Properties));

            AddProperty(new PreserveExtension(Properties));

            mDatePatternIndex = Properties.Count;
            AddRemoveBasedOnMode(rollingStyle.SelectedMode, mDatePatternModes, mDatePatternIndex, mDatePattern);

            mMaximumFileSizeIndex = Properties.Count;
            AddRemoveBasedOnMode(rollingStyle.SelectedMode, mMaximumFileSizeModes, mMaximumFileSizeIndex, mMaximumFileSize);

            AddProperty(new MaxSizeRollBackups(Properties));

            mCountDirectionIndex = Properties.Count;
            AddRemoveBasedOnMode(rollingStyle.SelectedMode, mCountDirectionModes, mCountDirectionIndex, mCountDirection);
        }

        private void RollingStyleOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RollingStyle.SelectedMode))
            {
                RollingMode selectedMode = ((RollingStyle)sender).SelectedMode;
                AddRemoveBasedOnMode(selectedMode, mDatePatternModes, mDatePatternIndex, mDatePattern);
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
