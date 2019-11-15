// Copyright © 2019 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Definitions.Appenders
{
    internal class SmtpAppender : BufferingAppenderSkeleton
    {
        public SmtpAppender(IElementConfiguration configuration)
            : base(configuration, true)
        {
        }

        public override AppenderDescriptor Descriptor => AppenderDescriptor.Smtp;

        public override string Name => "SMTP Appender";

        protected override void AddAppenderSpecificProperties()
        {
            base.AddAppenderSpecificProperties();

            AddProperty(new RequiredStringProperty(Properties, "Host:", "smtpHost"));
            //AddProperty(new Port(Properties, "Port:"));
            AddProperty(new SmtpAuthentication(Properties));
            AddProperty(new BooleanPropertyBase(Properties, "Enable SSL:", "enableSsl", false, false));
            AddProperty(new RequiredStringProperty(Properties, "Username:", "username"));
            AddProperty(new RequiredStringProperty(Properties, "Password:", "password")); //TODO obfuscate?
            AddProperty(new RequiredStringProperty(Properties, "To:", "to"));
            AddProperty(new RequiredStringProperty(Properties, "From:", "from"));
            AddProperty(new RequiredStringProperty(Properties, "Reply To:", "replyTo")); //TODO some of these are optional
            AddProperty(new RequiredStringProperty(Properties, "Cc:", "cc"));
            AddProperty(new RequiredStringProperty(Properties, "Bcc:", "bcc"));
            AddProperty(new RequiredStringProperty(Properties, "Subject:", "subject"));
            AddProperty(new Encoding(Properties, "Subject Encoding:"));
            AddProperty(new Encoding(Properties, "Body Encoding:"));
            AddProperty(new SmtpPriority(Properties));
        }
    }
}
