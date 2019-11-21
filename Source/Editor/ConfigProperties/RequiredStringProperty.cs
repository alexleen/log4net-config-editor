// Copyright © 2019 Alex Leendertsen

using System.Collections.ObjectModel;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    internal class RequiredStringProperty : StringValueProperty
    {
        public RequiredStringProperty(ReadOnlyCollection<IProperty> container, string name, string elementName)
            : base(container, name, elementName)
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
