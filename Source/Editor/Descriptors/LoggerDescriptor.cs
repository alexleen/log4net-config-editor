// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors.Base;

namespace Editor.Descriptors
{
    public class LoggerDescriptor : DescriptorBase
    {
        public static readonly LoggerDescriptor Root;

        static LoggerDescriptor()
        {
            Root = new LoggerDescriptor("Root");
        }

        public LoggerDescriptor(string name)
            : base(name)
        {
        }
    }
}
