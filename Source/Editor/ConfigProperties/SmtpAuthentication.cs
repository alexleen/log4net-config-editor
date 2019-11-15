using System.Collections.ObjectModel;
using System.Windows;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using log4net.Appender;

namespace Editor.ConfigProperties
{
    class SmtpAuthentication : EnumPropertyBase<SmtpAppender.SmtpAuthentication>
    {
        public SmtpAuthentication(ReadOnlyCollection<IProperty> container)
            : base(container, GridLength.Auto, "Authentication:", 100, "authentication")
        {
        }
    }
}
