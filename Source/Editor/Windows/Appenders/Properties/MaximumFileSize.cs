// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;

namespace Editor.Windows.Appenders.Properties
{
    public class MaximumFileSize : StringValueProperty
    {
        private const string MaximumFileSizeName = "maximumFileSize";
        private const string DefaultMaxFileSize = "10MB";

        public MaximumFileSize(ObservableCollection<IAppenderProperty> container)
            : base(container, GridLength.Auto, "Maximum File Size:")
        {
            Value = DefaultMaxFileSize;
            ToolTip = "The maximum size that the output file is allowed to reach before being rolled over to backup files. Must be suffixed with \"KB\", \"MB\", or \"GB\".";
        }

        public override void Load(XmlNode originalAppenderNode)
        {
            SetValueIfNotNullOrEmpty(originalAppenderNode.GetValueAttributeValueFromChildElement(MaximumFileSizeName));
        }

        public override bool TryValidate()
        {
            string trimmed = Value.Trim();
            if (!(trimmed.EndsWith("KB") || trimmed.EndsWith("MB") || trimmed.EndsWith("GB")))
            {
                MessageBox.Show("Maximum file size must end with either \"KB\", \"MB\", or \"GB\".");
                return false;
            }

            return base.TryValidate();
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newAppenderNode)
        {
            if (Value != DefaultMaxFileSize)
            {
                xmlDoc.CreateElementWithAttribute(MaximumFileSizeName, "value", Value.Trim()).AppendTo(newAppenderNode);
            }
        }
    }
}
