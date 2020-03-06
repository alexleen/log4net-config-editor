// Copyright © 2019 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    internal class RequiredStringProperty : StringValueProperty
    {
        public RequiredStringProperty(string name, string elementName)
            : base(name, elementName)
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
