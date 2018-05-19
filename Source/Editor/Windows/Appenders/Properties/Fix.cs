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
        private const string FixName = "Fix";

        public Fix(ObservableCollection<IAppenderProperty> container)
            : base(container, GridLength.Auto)
        {
            Fixes = Enum.GetValues(typeof(FixFlags)).Cast<FixFlags>();
        }

        public IEnumerable<FixFlags> Fixes { get; }

        public FixFlags SelectedFix { get; set; }

        public override void Load(XmlNode originalAppenderNode)
        {
            string fixValue = originalAppenderNode.GetValueAttributeValueFromChildElement(FixName);

            if (int.TryParse(fixValue, out int fixValueInt) && Enum.IsDefined(typeof(FixFlags), fixValueInt))
            {
                SelectedFix = (FixFlags)fixValueInt;
            }
            else
            {
                SelectedFix = FixFlags.None;
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newAppenderNode)
        {
            xmlDoc.CreateElementWithValueAttribute(FixName, ((int)SelectedFix).ToString()).AppendTo(newAppenderNode);
        }
    }
}
