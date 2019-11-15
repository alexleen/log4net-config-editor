using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Definitions.Appenders
{
    class SmtpAppender : BufferingAppenderSkeleton
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

            //SmtpHost
            //AddProperty(new Port(Properties, "Port:"));
            //Authentication (enum)
            //EnableSsl
            //Username
            //Password
            //To
            //From
            //ReplyTo
            //Cc
            //Bcc
            //Subject
            AddProperty(new Encoding(Properties, "Subject Encoding:"));
            AddProperty(new Encoding(Properties, "Body Encoding:"));
            //Priority
        }
    }
}
