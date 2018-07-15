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
        private readonly IElementConfiguration mAppenderConfiguration;
        private const string NameName = "name";

        public Name(ReadOnlyCollection<IProperty> container, IElementConfiguration appenderConfiguration)
            : base(container, "Name:", NameName)
        {
            mAppenderConfiguration = appenderConfiguration;
            IsFocused = true;
        }

        public override void Load(XmlNode originalNode)
        {
            SetValueIfNotNullOrEmpty(originalNode.Attributes?[NameName]?.Value);
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            if (string.IsNullOrEmpty(Value))
            {
                messageBoxService.ShowError("A name must be assigned to this appender.");
                return false;
            }

            foreach (XmlNode appender in mAppenderConfiguration.Log4NetNode.SelectNodes("appender"))
            {
                if (!Equals(appender, mAppenderConfiguration.OriginalNode) && appender.Attributes?[NameName].Value == Value)
                {
                    messageBoxService.ShowError("Name must be unique.");
                    return false;
                }
            }

            return base.TryValidate(messageBoxService);
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            newNode.AppendAttribute(xmlDoc, NameName, Value);
        }
    }
}
