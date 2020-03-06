// Copyright © 2020 Alex Leendertsen

using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    public class BufferSize : StringValueProperty
    {
        private const string BufferSizeName = "bufferSize";
        private readonly string mDefaultBufferSize;

        public BufferSize(int defaultSize)
            : base("Buffer Size:", BufferSizeName)
        {
            mDefaultBufferSize = defaultSize.ToString();
            Value = mDefaultBufferSize;
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
            if (Value != mDefaultBufferSize)
            {
                xmlDoc.CreateElementWithValueAttribute(BufferSizeName, Value).AppendTo(newNode);
            }
        }
    }
}
