// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    public class ApplicationName : StringValueProperty
    {
        public ApplicationName(ReadOnlyCollection<IProperty> container)
            : base(container, "Application Name:", null)
        {
        }

        public override void Load(XmlNode originalNode)
        {
            SetValueIfNotNullOrEmpty(originalNode.SelectSingleNode("param[@name='ApplicationName']")?.Attributes?["value"]?.Value);
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            if (string.IsNullOrEmpty(Value))
            {
                messageBoxService.ShowError("An application name must be assigned to this appender.");
                return false;
            }

            return base.TryValidate(messageBoxService);
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            xmlDoc.CreateElementWithAttributes("param",
                                               new[]
                                               {
                                                   (Name: "name", Value: "ApplicationName"),
                                                   (Name: "value", Value)
                                               })
                  .AppendTo(newNode);
        }
    }
}
