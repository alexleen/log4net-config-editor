// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    public class Key : StringValueProperty
    {
        public Key(ReadOnlyCollection<IProperty> container)
            : base(container, "Key:", "key")
        {
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            if (string.IsNullOrEmpty(Value))
            {
                messageBoxService.ShowError("'Key' must be specified.");
                return false;
            }

            return base.TryValidate(messageBoxService);
        }
    }
}
