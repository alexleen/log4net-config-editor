// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using log4net.Core;

namespace Editor.Windows.Appenders.Properties
{
    public class Fix : AppenderPropertyBase
    {
        public Fix(ObservableCollection<IAppenderProperty> container, XmlNode appenderNode)
            : base(container, GridLength.Auto, appenderNode)
        {
            Fixes = Enum.GetValues(typeof(FixFlags)).Cast<FixFlags>();
        }

        public IEnumerable<FixFlags> Fixes { get; }

        public FixFlags SelectedFix { get; set; }

        public override void Load()
        {
            string fixValue = AppenderNode["Fix"]?.Attributes["value"].Value;

            if (int.TryParse(fixValue, out int fixValueInt) && Enum.IsDefined(typeof(FixFlags), fixValueInt))
            {
                SelectedFix = (FixFlags)fixValueInt;
            }
            else
            {
                SelectedFix = FixFlags.None;
            }
        }

        public override void Save(XmlDocument xmlDoc)
        {
            xmlDoc.CreateElementWithAttribute("Fix", "value", ((int)SelectedFix).ToString()).AppendTo(AppenderNode);
        }
    }
}
