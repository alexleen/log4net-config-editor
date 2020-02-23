// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    public class MaxSizeRollBackups : StringValueProperty
    {
        public MaxSizeRollBackups()
            : base("Max Size Roll Backups:", "maxSizeRollBackups")
        {
            ToolTip = "The maximum number of backup files that are kept before the oldest is erased.";
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            if (!int.TryParse(Value, out int _))
            {
                messageBoxService.ShowError("Max size roll backups must be a valid integer.");
                return false;
            }

            return base.TryValidate(messageBoxService);
        }
    }
}
