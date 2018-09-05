// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    public class LogName : StringValueProperty
    {
        public LogName(ReadOnlyCollection<IProperty> container)
            : base(container, "Log Name:", "logName")
        {
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            if (string.IsNullOrEmpty(Value))
            {
                messageBoxService.ShowError("A log name must be assigned to this appender.");
                return false;
            }

            return base.TryValidate(messageBoxService);
        }
    }
}
