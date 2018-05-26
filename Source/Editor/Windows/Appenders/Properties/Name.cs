// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Appenders.Properties
{
    public class Name : StringValueProperty
    {
        private readonly XmlNode mLog4NetNode;
        private const string NameName = "name";

        public Name(ObservableCollection<IProperty> container, XmlNode log4NetNode)
            : base(container, GridLength.Auto, "Name:")
        {
            mLog4NetNode = log4NetNode;
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

            foreach (XmlNode appender in mLog4NetNode.SelectNodes("appender"))
            {
                if (appender.Attributes?[NameName].Value == Value)
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
