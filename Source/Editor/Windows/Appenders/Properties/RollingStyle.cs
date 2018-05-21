// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.PropertyCommon;
using log4net.Appender;

namespace Editor.Windows.Appenders.Properties
{
    public class RollingStyle : PropertyBase
    {
        private const string RollingStyleName = "rollingStyle";

        public RollingStyle(ObservableCollection<IProperty> container)
            : base(container, GridLength.Auto)
        {
            Modes = Enum.GetValues(typeof(RollingFileAppender.RollingMode)).Cast<RollingFileAppender.RollingMode>();
            SelectedMode = RollingFileAppender.RollingMode.Composite;
        }

        public IEnumerable<RollingFileAppender.RollingMode> Modes { get; }

        private RollingFileAppender.RollingMode mSelectedMode;

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

        public override void Load(XmlNode originalNode)
        {
            string modeValue = originalNode.GetValueAttributeValueFromChildElement(RollingStyleName);
            if (!string.IsNullOrEmpty(modeValue) && Enum.TryParse(modeValue, out RollingFileAppender.RollingMode mode))
            {
                SelectedMode = mode;
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            //Composite is the default and does not need to be specified in the XML if chosen
            if (SelectedMode != RollingFileAppender.RollingMode.Composite)
            {
                xmlDoc.CreateElementWithValueAttribute(RollingStyleName, SelectedMode.ToString()).AppendTo(newNode);
            }
        }
    }
}
