// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors.Base;
using Editor.Enums;
using Editor.Utilities;

namespace Editor.Descriptors
{
    public class LoggerDescriptor : DescriptorBase
    {
        public static readonly LoggerDescriptor Root, Logger;

        static LoggerDescriptor()
        {
            Root = new LoggerDescriptor("Root", Log4NetXmlConstants.Root, LoggerType.Root);
            Logger = new LoggerDescriptor("Logger", Log4NetXmlConstants.Logger, LoggerType.Logger);
        }

        private LoggerDescriptor(string name, string elementName, LoggerType loggerType)
            : base(name, elementName, null)
        {
            LoggerType = loggerType;
        }

        public LoggerType LoggerType { get; }
    }
}
