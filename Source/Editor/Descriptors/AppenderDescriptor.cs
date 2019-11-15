// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using Editor.Descriptors.Base;
using Editor.Enums;

namespace Editor.Descriptors
{
    public class AppenderDescriptor : DescriptorBase
    {
        public static readonly AppenderDescriptor Console, File, RollingFile, EventLog, Async, Forwarding, ManagedColor, Udp, LocalSyslog, RemoteSyslog, Smtp;
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
            Udp = new AppenderDescriptor("UDP", AppenderType.Udp, "log4net.Appender.UdpAppender");
            LocalSyslog = new AppenderDescriptor("Local Syslog", AppenderType.LocalSyslog, "log4net.Appender.LocalSyslogAppender");
            RemoteSyslog = new AppenderDescriptor("Remote Syslog", AppenderType.RemoteSyslog, "log4net.Appender.RemoteSyslogAppender");
            Smtp = new AppenderDescriptor("SMTP", AppenderType.Smtp, "log4net.Appender.SmtpAppender");

            sDescriptorsByTypeNamespace = new Dictionary<string, AppenderDescriptor>
            {
                { Console.TypeNamespace, Console },
                { File.TypeNamespace, File },
                { RollingFile.TypeNamespace, RollingFile },
                { EventLog.TypeNamespace, EventLog },
                { Async.TypeNamespace, Async },
                { Forwarding.TypeNamespace, Forwarding },
                { ManagedColor.TypeNamespace, ManagedColor },
                { Udp.TypeNamespace, Udp },
                { LocalSyslog.TypeNamespace, LocalSyslog },
                { RemoteSyslog.TypeNamespace, RemoteSyslog },
                { Smtp.TypeNamespace, Smtp }
            };
        }

        private AppenderDescriptor(string name, AppenderType type, string typeNamespace)
            : base(name, "appender", typeNamespace)
        {
            Type = type;
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

        public AppenderType Type { get; }
    }
}
