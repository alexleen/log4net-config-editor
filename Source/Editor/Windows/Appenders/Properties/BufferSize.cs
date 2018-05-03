// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;

namespace Editor.Windows.Appenders.Properties
{
    public class BufferSize : StringValueProperty
    {
        private const string DefaultBufferSize = "1000";

        public BufferSize(ObservableCollection<IAppenderProperty> container)
            : base(container, GridLength.Auto, "Buffer Size:")
        {
        }

        public override void Load(XmlNode originalAppenderNode)
        {
            string bufferSizeStr = originalAppenderNode["bufferSize"]?.Attributes["value"].Value;

            if (int.TryParse(bufferSizeStr, out int _))
            {
                Value = bufferSizeStr;
            }
            else
            {
                Value = DefaultBufferSize;
            }
        }

        public override bool TryValidate()
        {
            if (!int.TryParse(Value, out int _))
            {
                MessageBox.Show("Buffer size must be a valid integer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return base.TryValidate();
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newAppenderNode)
        {
            if (Value != DefaultBufferSize)
            {
                xmlDoc.CreateElementWithAttribute("bufferSize", "value", Value).AppendTo(newAppenderNode);
            }
        }
    }
}
