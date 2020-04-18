// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    internal class Aditivity : BooleanPropertyBase
    {
        private const string ElementName = "aditivity";

        public Aditivity()
            : base("Aditivity:", ElementName, true)
        {
        }

        public override void Load(IElementConfiguration config)
        {
            if (config.Load(ElementName, out IValueResult result) && bool.TryParse(result.AttributeValue, out bool value))
            {
                Value = value;
            }
        }

        public override void Save(IElementConfiguration config)
        {
            if (!Value)
            {
                config.Save(ElementName, Value.ToString());
            }
        }
    }
}
