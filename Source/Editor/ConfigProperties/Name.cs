// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    public class Name : StringValueProperty
    {
        private readonly IElementConfiguration mConfiguration;

        public Name(ReadOnlyCollection<IProperty> container, IElementConfiguration configuration)
            : base(container, "Name:", Log4NetXmlConstants.Name)
        {
            mConfiguration = configuration;
            IsFocused = true;
        }

        public override void Load(XmlNode originalNode)
        {
            SetValueIfNotNullOrEmpty(originalNode.Attributes[Log4NetXmlConstants.Name]?.Value);
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            if (string.IsNullOrEmpty(Value))
            {
                messageBoxService.ShowError("A name must be assigned to this appender.");
                return false;
            }

            foreach (XmlNode appender in mConfiguration.Log4NetNode.SelectNodes("appender"))
            {
                if (!Equals(appender, mConfiguration.OriginalNode) && appender.Attributes[Log4NetXmlConstants.Name]?.Value == Value)
                {
                    messageBoxService.ShowError("Name must be unique.");
                    return false;
                }
            }

            return base.TryValidate(messageBoxService);
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            newNode.AppendAttribute(xmlDoc, Log4NetXmlConstants.Name, Value);
        }

        /// <summary>
        /// Original name value. Null if no original name (i.e. new appender).
        /// </summary>
        public string OriginalName => mConfiguration.OriginalNode?.Attributes?[Log4NetXmlConstants.Name]?.Value;

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
    }
}
