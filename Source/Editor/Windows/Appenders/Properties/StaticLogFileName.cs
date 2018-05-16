// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;

namespace Editor.Windows.Appenders.Properties
{
    public class StaticLogFileName : AppenderPropertyBase
    {
        private const string StaticLogFileNameName = "staticLogFileName";

        public StaticLogFileName(ObservableCollection<IAppenderProperty> container)
            : base(container, GridLength.Auto)
        {
        }

        public bool Value { get; set; }

        public override void Load(XmlNode originalAppenderNode)
        {
            string valueStr = originalAppenderNode.GetValueAttributeValueFromChildElement(StaticLogFileNameName);
            if (bool.TryParse(valueStr, out bool value))
            {
                Value = value;
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newAppenderNode)
        {
            //Default is off - only save if on
            if (Value)
            {
                xmlDoc.CreateElementWithValueAttribute(StaticLogFileNameName, Value.ToString().ToLower()).AppendTo(newAppenderNode);
            }
        }
    }
}
