// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    public class DatePattern : StringValueProperty
    {
        public DatePattern(ReadOnlyCollection<IProperty> container)
            : base(container, "Date Pattern:", "datePattern")
        {
            ToolTip = "This property determines the rollover schedule when rolling over on date.";
        }

        public override bool TryValidate(IMessageBoxService messageBoxService)
        {
            if (string.IsNullOrEmpty(Value))
            {
                messageBoxService.ShowError("A valid date pattern must be assigned.");
                return false;
            }

            return base.TryValidate(messageBoxService);
        }
    }
}
