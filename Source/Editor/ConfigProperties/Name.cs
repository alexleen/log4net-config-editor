// Copyright © 2020 Alex Leendertsen

using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    /// <summary>
    /// Represents the name attribute of an appender, logger, or param.
    /// </summary>
    public class Name : StringValueProperty
    {
        private readonly IElementConfiguration mConfiguration;

        public Name(IElementConfiguration configuration)
            : base("Name:", null)
        {
            mConfiguration = configuration;
            IsFocused = true;
        }

        /// <summary>
        /// Original name value. Null if no original name (i.e. new appender).
        /// </summary>
        public string OriginalName => mConfiguration.OriginalNode?.FindNodeAttributeValue(Log4NetXmlConstants.Name);

        /// <summary>
        /// Whether or not the value for the Name property has changed.
        /// Null if there was no original appender (and therefore no original name).
        /// True if there was an original appender and the name has changed.
        /// False otherwise.
        /// </summary>
        public bool? Changed
        {
            get
            {
                if (mConfiguration.OriginalNode == null)
                {
                    return null;
                }

                return Value != OriginalName;
            }
        }

        public override void Load(IElementConfiguration config)
        {
            if (config.Load(Log4NetXmlConstants.Name, out IValueResult result))
            {
                Value = result.AttributeValue;
            }
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            if (string.IsNullOrEmpty(Value))
            {
                messageBoxService.ShowError("A name must be assigned to this appender.");
                return false;
            }

            foreach (XmlNode appender in mConfiguration.FindLog4NetNodeChildren("appender"))
            {
                if (!Equals(appender, mConfiguration.OriginalNode) && appender.FindNodeAttributeValue(Log4NetXmlConstants.Name) == Value)
                {
                    messageBoxService.ShowError("Name must be unique.");
                    return false;
                }
            }

            return base.TryValidate(messageBoxService);
        }

        public override void Save(IElementConfiguration config)
        {
            config.Save(Log4NetXmlConstants.Name, Value);
        }
    }
}
