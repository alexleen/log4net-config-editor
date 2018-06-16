// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using Editor.Enums;

namespace Editor.Descriptors
{
    public class AppenderDescriptor
    {
        public static readonly AppenderDescriptor Console, File, RollingFile, EventLog, Async, Forwarding, ManagedColor;
        private static readonly IDictionary<string, AppenderDescriptor> sDescriptorsByTypeNamespace;

        static AppenderDescriptor()
        {
            Console = new AppenderDescriptor("Console", AppenderType.Console, "log4net.Appender.ConsoleAppender");
            File = new AppenderDescriptor("File", AppenderType.File, "log4net.Appender.FileAppender");
            RollingFile = new AppenderDescriptor("Rolling File", AppenderType.RollingFile, "log4net.Appender.RollingFileAppender");
            EventLog = new AppenderDescriptor("Event Log", AppenderType.EventLog, "log4net.Appender.EventLogAppender");
            Async = new AppenderDescriptor("Async", AppenderType.Async, "Log4Net.Async.AsyncForwardingAppender,Log4Net.Async");
            Forwarding = new AppenderDescriptor("Forwarding", AppenderType.Forwarding, "log4net.Appender.ForwardingAppender");
            ManagedColor = new AppenderDescriptor("Managed Color", AppenderType.ManagedColor, "log4net.Appender.ManagedColoredConsoleAppender");

            sDescriptorsByTypeNamespace = new Dictionary<string, AppenderDescriptor>
            {
                { Console.TypeNamespace, Console },
                { File.TypeNamespace, File },
                { RollingFile.TypeNamespace, RollingFile },
                { EventLog.TypeNamespace, EventLog },
                { Async.TypeNamespace, Async },
                { Forwarding.TypeNamespace, Forwarding },
                { ManagedColor.TypeNamespace, ManagedColor }
            };
        }

        private AppenderDescriptor(string name, AppenderType type, string typeNamespace)
        {
            Name = name;
            Type = type;
            TypeNamespace = typeNamespace;
        }

        public static bool TryFindByTypeNamespace(string typeNamespace, out AppenderDescriptor appender)
        {
            if (typeNamespace == null)
            {
                appender = null;
                return false;
            }

            return sDescriptorsByTypeNamespace.TryGetValue(typeNamespace, out appender);
        }

        public string Name { get; }

        public AppenderType Type { get; }

        public string TypeNamespace { get; }
    }
}
