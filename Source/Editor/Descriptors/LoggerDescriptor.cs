// Copyright © 2018 Alex Leendertsen

namespace Editor.Descriptors
{
    public class LoggerDescriptor
    {
        public static readonly LoggerDescriptor Root;

        static LoggerDescriptor()
        {
            Root = new LoggerDescriptor("Root");
        }

        public LoggerDescriptor(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
