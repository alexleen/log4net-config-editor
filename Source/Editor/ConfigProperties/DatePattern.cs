// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
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

            //https://stackoverflow.com/questions/62771/how-do-i-check-if-a-given-string-is-a-legal-valid-file-name-under-windows#comment52147867_62888
            Regex unspupportedRegex = new Regex("(^(PRN|AUX|NUL|CON|COM[1-9]|LPT[1-9]|(\\.+)$)(\\..*)?$)|(([\\x00-\\x1f\\\\?*:\";‌​|/<>])+)|([\\. ]+)", RegexOptions.IgnoreCase);

            Match m = unspupportedRegex.Match(Value);

            if (m.Success)
            {
                messageBoxService.ShowError($"Date pattern must not contain invalid characters: '{m.Value}'");
                return false;
            }

            return base.TryValidate(messageBoxService);
        }
    }
}
