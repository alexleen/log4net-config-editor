// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;

namespace Editor.Windows.Appenders.Properties
{
    public class PreserveExtension : AppenderPropertyBase
    {
        private const string PreserveExtensionName = "preserveLogFileNameExtension";

        public PreserveExtension(ObservableCollection<IAppenderProperty> container)
            : base(container, GridLength.Auto)
        {
        }

        public bool Preserve { get; set; }

        public override void Load(XmlNode originalAppenderNode)
        {
            string preserveStr = originalAppenderNode.GetValueAttributeValueFromChildElement(PreserveExtensionName);
            if (!string.IsNullOrEmpty(preserveStr) && bool.TryParse(preserveStr, out bool preserve))
            {
                Preserve = preserve;
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newAppenderNode)
        {
            if (Preserve)
            {
                xmlDoc.CreateElementWithAttribute(PreserveExtensionName, "value", Preserve.ToString().ToLower()).AppendTo(newAppenderNode);
            }
        }
    }
}
