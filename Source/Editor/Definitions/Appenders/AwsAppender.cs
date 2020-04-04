// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;

namespace Editor.Definitions.Appenders
{
    internal class AwsAppender : AppenderSkeleton
    {
        public AwsAppender(IElementConfiguration configuration, bool requiresLayout = true)
            : base(configuration, requiresLayout)
        {
        }

        public override string Name => "AWS Appender";
        public override AppenderDescriptor Descriptor => AppenderDescriptor.AwsAppender;

        protected override void AddAppenderSpecificProperties()
        {
            AddProperty(new StringValueProperty("Log Group:", "logGroup"));
            AddProperty(new BooleanPropertyBase("Disable Log Group Creation:", "disableLogGroupCreation", false));
            AddProperty(new StringValueProperty("Profile:", "profile"));
            AddProperty(new StringValueProperty("Profiles Location:", "profilesLocation"));
            AddProperty(new StringValueProperty("Credentials:", "credentials")); //TODO what type is this actually?
            AddProperty(new StringValueProperty("Region:", "region"));
            AddProperty(new StringValueProperty("Service URL:", "serviceUrl"));
            AddProperty(new StringValueProperty("Batch Push Interval:", "BatchPushInterval")); //TODO this is a TimeSpan - how does that get parsed?
            AddProperty(new StringValueProperty("Batch Size In Bytes:", "batchSizeInBytes")); //TODO this is an int
            AddProperty(new StringValueProperty("Max Queued Messages:", "maxQueuedMessages")); //TODO this is an int
            AddProperty(new StringValueProperty("Log Stream Name Suffix:", "logStreamNameSuffix"));
            AddProperty(new StringValueProperty("Log Stream Name Prefix:", "logStreamNamePrefix"));
            AddProperty(new StringValueProperty("Library Log File Name:", "libraryLogFileName"));
        }
    }
}
