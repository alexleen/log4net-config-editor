// Copyright © 2019 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Windows;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    internal class SmtpPriority : EnumPropertyBase<MailPriority>
    {
        public SmtpPriority(ReadOnlyCollection<IProperty> container)
            : base(container, GridLength.Auto, "Priority:", 75, "priority")
        {
        }
    }
}
