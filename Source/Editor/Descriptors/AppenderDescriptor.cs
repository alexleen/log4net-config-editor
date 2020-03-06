// Copyright © 2020 Alex Leendertsen

using System.Collections.Generic;
using Editor.Descriptors.Base;
using Editor.Enums;

namespace Editor.Descriptors
{
    public class AppenderDescriptor : DescriptorBase
    {
        public static readonly AppenderDescriptor Console, File, RollingFile, EventLog, Async, Forwarding, ManagedColor, Udp, LocalSyslog, RemoteSyslog, Smtp, Debug, Trace, Telnet, Remoting, OutputDebugString, NetSend, Memory;
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
            Debug = new AppenderDescriptor("Debug", AppenderType.Debug, "log4net.Appender.DebugAppender");
            Trace = new AppenderDescriptor("Trace", AppenderType.Trace, "log4net.Appender.TraceAppender");
            Telnet = new AppenderDescriptor("Telnet", AppenderType.Telnet, "log4net.Appender.TelnetAppender");
            Remoting = new AppenderDescriptor("Remoting", AppenderType.Remoting, "log4net.Appender.RemotingAppender");
            OutputDebugString = new AppenderDescriptor("Output Debug String", AppenderType.OutputDebugString, "log4net.Appender.OutputDebugStringAppender");
            NetSend = new AppenderDescriptor("Net Send", AppenderType.NetSend, "log4net.Appender.NetSendAppender");
            Memory = new AppenderDescriptor("Memory", AppenderType.Memory, "log4net.Appender.MemoryAppender");

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
                { Smtp.TypeNamespace, Smtp },
                { Debug.TypeNamespace, Debug },
                { Trace.TypeNamespace, Trace },
                { Telnet.TypeNamespace, Telnet },
                { Remoting.TypeNamespace, Remoting },
                { OutputDebugString.TypeNamespace, OutputDebugString },
                { NetSend.TypeNamespace, NetSend },
                { Memory.TypeNamespace, Memory }
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
