// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Utilities;

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
            AddProperty(new StringValueProperty("Log Group:", "logGroup")
            {
                ToolTip = "This is the name of the Cloud Watch Logs group where streams will be created and log messages written to."
            });
            AddProperty(new BooleanPropertyBase("Disable Log Group Creation:", "disableLogGroupCreation", false)
            {
                ToolTip = "Determines whether or not to create a new Log Group, if the one specified by Log Group doesn't already exist."
            });
            AddProperty(new StringValueProperty("Profile:", "profile")
            {
                ToolTip = "The profile is used to look up AWS credentials in the profile store."
            });
            AddProperty(new StringValueProperty("Profiles Location:", "profilesLocation")
            {
                ToolTip = "If this is not set the default profile store is used by the AWS SDK for .NET to look up credentials.\n" +
                          "This is most commonly used when you are running an application of on-premise under a service account."
            });
            AddProperty(new StringValueProperty("Credentials:", "credentials", Log4NetXmlConstants.Type)
            {
                ToolTip = "These are the AWS credentials used by the AWS SDK for .NET to make service calls."
            });
            AddProperty(new StringValueProperty("Region:", "region")
            {
                ToolTip = "This is the AWS Region that will be used for Cloud Watch Logs. If this is not the AWS SDK for .NET will use its fall back logic to try and determine the region through environment variables and EC2 instance metadata.\n" +
                          "If the Region is not set and no region is found by the SDK's fall back logic then an exception will be thrown."
            });
            AddProperty(new StringValueProperty("Service URL:", "serviceUrl")
            {
                ToolTip = "This is an optional property; change it only if you want to try a different service endpoint. Ex. for Local Stack"
            });
            AddProperty(new StringValueProperty("Batch Push Interval:", "batchPushInterval")
            {
                ToolTip = "For performance the log messages are sent to AWS in batch sizes. Batch Push Interval dictates the frequency of when batches are sent.\n" +
                          "If either Batch Push Interval or Batch Size In Bytes are exceeded the batch will be sent. Value must be parsable as a TimeSpan."
            }); //TODO this is a TimeSpan - I think parsed by the parse method. Check validity.
            AddProperty(new NumericProperty<int>("Batch Size In Bytes:", "batchSizeInBytes", 102400)
            {
                ToolTip = "For performance the log messages are sent to AWS in batch sizes. BatchSizeInBytes dictates the total size of the batch in bytes when batches are sent.\n" +
                          "If either BatchPushInterval or BatchSizeInBytes are exceeded the batch will be sent."
            });
            AddProperty(new NumericProperty<int>("Max Queued Messages:", "maxQueuedMessages", 10000)
            {
                ToolTip = "This specifies the maximum number of log messages that could be stored in-memory. MaxQueuedMessages dictates the total number of log messages that can be stored in-memory.\n" +
                          "If this exceeded, incoming log messages will be dropped."
            });
            AddProperty(new StringValueProperty("Log Stream Name Suffix:", "logStreamNameSuffix")
            {
                ToolTip = "The Log StreamName consists of an optional user-defined prefix segment, then a DateTimeStamp as the system-defined prefix segment, and a user defined suffix value that can be set using the Log Stream Name Suffix property defined here.\n" +
                          "The default is going to a Guid."
            });
            AddProperty(new StringValueProperty("Log Stream Name Prefix:", "logStreamNamePrefix")
            {
                ToolTip = "The Log Stream Name consists of an optional user-defined prefix segment (defined here), then a DateTimeStamp as the system-defined prefix segment, and a user defined suffix value that can be set using the LogStreamNameSuffix property.\n" +
                          "The default will use an empty string for this user-defined portion, meaning the log stream name will start with the system-defined portion of the prefix (yyyy/MM/dd ... )"
            });
            AddProperty(new StringValueProperty("Library Log File Name:", "libraryLogFileName")
            {
                Value = "aws-logger-errors.txt",
                ToolTip = "This is the name of the file into which errors from the AWS.Logger.Core library will be written into.\n" +
                          "The default is going to be: aws-logger-errors.txt."
            });
        }
    }
}
