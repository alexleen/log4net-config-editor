// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.Appenders.Properties;

namespace Editor.Windows.Filters.Properties
{
    public class AcceptOnMatch : AppenderPropertyBase
    {
        private const string AcceptOnMatchName = "acceptOnMatch";

        public AcceptOnMatch(ObservableCollection<IAppenderProperty> container)
            : base(container, GridLength.Auto)
        {
        }

        public bool Accept { get; set; } = true;

        public override void Load(XmlNode originalAppenderNode)
        {
            string acceptStr = originalAppenderNode.GetValueAttributeValueFromChildElement(AcceptOnMatchName);
            if (!string.IsNullOrEmpty(acceptStr) && bool.TryParse(acceptStr, out bool accept))
            {
                Accept = accept;
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newAppenderNode)
        {
            if (!Accept)
            {
                xmlDoc.CreateElementWithValueAttribute(AcceptOnMatchName, "false");
            }
        }
    }
}
