// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Appenders.Properties
{
    public class BufferSize : StringValueProperty
    {
        private const string DefaultBufferSize = "1000";
        private const string BufferSizeName = "bufferSize";

        public BufferSize(ObservableCollection<IProperty> container)
            : base(container, GridLength.Auto, "Buffer Size:")
        {
            Value = DefaultBufferSize;
        }

        public override void Load(XmlNode originalNode)
        {
            string bufferSizeStr = originalNode.GetValueAttributeValueFromChildElement(BufferSizeName);

            if (int.TryParse(bufferSizeStr, out int _))
            {
                Value = bufferSizeStr;
            }
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            if (!int.TryParse(Value, out int _))
            {
                messageBoxService.ShowError("Buffer size must be a valid integer.");
                return false;
            }

            return base.TryValidate(messageBoxService);
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (Value != DefaultBufferSize)
            {
                xmlDoc.CreateElementWithValueAttribute(BufferSizeName, Value).AppendTo(newNode);
            }
        }
    }
}
