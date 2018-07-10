// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors.Base;
using Editor.Enums;

namespace Editor.Descriptors
{
    public class LoggerDescriptor : DescriptorBase
    {
        public static readonly LoggerDescriptor Root, Logger;

        static LoggerDescriptor()
        {
            Root = new LoggerDescriptor("Root", "root", LoggerType.Root);
            Logger = new LoggerDescriptor("Logger", "logger", LoggerType.Logger);
        }

        public LoggerDescriptor(string name, string elementName, LoggerType loggerType)
            : base(name, elementName)
        {
            LoggerType = loggerType;
        }

        public LoggerType LoggerType { get; }
    }
}
