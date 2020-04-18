// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    internal class Value : StringValueProperty
    {
        internal Value()
            : base("Value:", null)
        {
        }

        public override void Load(IElementConfiguration config)
        {
            if (config.Load(Log4NetXmlConstants.Value, out IValueResult result))
            {
                SetValueIfNotNullOrEmpty(result.AttributeValue);
            }
        }

        public override void Save(IElementConfiguration config)
        {
            if (!string.IsNullOrEmpty(Value))
            {
                config.Save(Log4NetXmlConstants.Value, Value);
            }
        }
    }
}
