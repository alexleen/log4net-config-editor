// Copyright © 2020 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;
using log4net.Appender;

namespace Editor.ConfigProperties
{
    public class RollingStyle : PropertyBase
    {
        private const string RollingStyleName = "rollingStyle";

        private RollingFileAppender.RollingMode mSelectedMode;

        public RollingStyle()
            : base(GridLength.Auto)
        {
            Modes = Enum.GetValues(typeof(RollingFileAppender.RollingMode)).Cast<RollingFileAppender.RollingMode>();
            SelectedMode = RollingFileAppender.RollingMode.Composite;
        }

        public IEnumerable<RollingFileAppender.RollingMode> Modes { get; }

        public RollingFileAppender.RollingMode SelectedMode
        {
            get => mSelectedMode;
            set
            {
                if (value == mSelectedMode)
                {
                    return;
                }

                mSelectedMode = value;
                OnPropertyChanged();
            }
        }

        public override void Load(IElementConfiguration config)
        {
            config.Load(Log4NetXmlConstants.Value, out IValueResult result, RollingStyleName);
            if (!string.IsNullOrEmpty(result.AttributeValue) && Enum.TryParse(result.AttributeValue, out RollingFileAppender.RollingMode mode))
            {
                SelectedMode = mode;
            }
        }

        public override void Save(IElementConfiguration config)
        {
            //Composite is the default and does not need to be specified in the XML if chosen
            if (SelectedMode != RollingFileAppender.RollingMode.Composite)
            {
                config.Save((RollingStyleName, Log4NetXmlConstants.Value, SelectedMode.ToString()));
            }
        }
    }
}
