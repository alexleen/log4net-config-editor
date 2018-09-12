// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    public class ApplicationName : StringValueProperty
    {
        public ApplicationName(ReadOnlyCollection<IProperty> container)
            : base(container, "Application Name:", "applicationName")
        {
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
    }
}
