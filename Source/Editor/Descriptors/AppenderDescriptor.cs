// Copyright © 2018 Alex Leendertsen

using Editor.Enums;
using Editor.Models;

namespace Editor.Descriptors
{
    public class AppenderDescriptor
    {
        public static AppenderDescriptor Console, File, RollingFile, EventLog, Async;

        static AppenderDescriptor()
        {
            Console = new AppenderDescriptor("Console", AppenderType.Console);
            File = new AppenderDescriptor("File", AppenderType.File);
            RollingFile = new AppenderDescriptor("Rolling File", AppenderType.RollingFile);
            EventLog = new AppenderDescriptor("Event Log", AppenderType.EventLog);
            Async = new AppenderDescriptor("Async", AppenderType.Async);
        }

        public AppenderDescriptor(string name, AppenderType type)
        {
            Name = name;
            Type = type;
        }

        public static bool TryFindByTypeNamespace(string typeNamespace, out AppenderDescriptor appender)
        {
            switch (typeNamespace)
            {
                case "log4net.Appender.ConsoleAppender":
                    appender = Console;
                    break;
                case "log4net.Appender.EventLogAppender":
                    appender = EventLog;
                    break;
                case "log4net.Appender.FileAppender":
                    appender = File;
                    break;
                case "Log4Net.Async.AsyncForwardingAppender,Log4Net.Async":
                    appender = Async;
                    break;
                default:
                    appender = null;
                    return false;
            }

            return true;
        }

        public string Name { get; }

        public AppenderType Type { get; }
    }
}
