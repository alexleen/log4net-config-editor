// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    public class LoggerToMatch : StringValueProperty
    {
        public LoggerToMatch(ReadOnlyCollection<IProperty> container)
            : base(container, "Logger to Match:", "loggerToMatch")
        {
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            if (string.IsNullOrEmpty(Value))
            {
                messageBoxService.ShowError("'Logger to Match' must be specified.");
                return false;
            }

            return base.TryValidate(messageBoxService);
        }
    }
}
