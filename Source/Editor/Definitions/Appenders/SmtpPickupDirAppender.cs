// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Definitions.Appenders
{
    internal class SmtpPickupDirAppender : BufferingAppenderSkeleton
    {
        internal SmtpPickupDirAppender(IElementConfiguration configuration)
            : base(configuration, true)
        {
        }

        public override AppenderDescriptor Descriptor => AppenderDescriptor.SmtpPickupDir;

        public override string Name => "SMTP Pickup Dir Appender";

        protected override void AddAppenderSpecificProperties()
        {
            base.AddAppenderSpecificProperties();
            AddProperty(new RequiredStringProperty("File Extension:", "fileExtension"));
            AddProperty(new RequiredStringProperty("Pickup Dir:", "pickupDir"));
            AddProperty(new RequiredStringProperty("To:", "to"));
            AddProperty(new RequiredStringProperty("From:", "from"));
            AddProperty(new RequiredStringProperty("Subject:", "subject"));
        }
    }
}
