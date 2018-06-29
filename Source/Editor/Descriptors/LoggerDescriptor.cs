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
            Root = new LoggerDescriptor("Root", LoggerType.Root);
            Logger = new LoggerDescriptor("Logger", LoggerType.Logger);
        }

        public LoggerDescriptor(string name, LoggerType loggerType)
            : base(name)
        {
            LoggerType = loggerType;
        }

        public LoggerType LoggerType { get; }
    }
}
