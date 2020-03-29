// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    internal class RequiredStringProperty : StringValueProperty
    {
        public RequiredStringProperty(string name, string elementName, string attributeName = Log4NetXmlConstants.Value)
            : base(name, elementName, attributeName)
        {
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            if (string.IsNullOrEmpty(Value))
            {
                messageBoxService.ShowError($"'{Name.TrimEnd(':')}' must be specified.");
                return false;
            }

            return base.TryValidate(messageBoxService);
        }
    }
}
