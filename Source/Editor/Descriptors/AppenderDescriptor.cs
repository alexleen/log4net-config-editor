// Copyright © 2020 Alex Leendertsen

using System.Collections.Generic;
using Editor.Descriptors.Base;
using Editor.Enums;

namespace Editor.Descriptors
{
    public class AppenderDescriptor : DescriptorBase
    {
        public static readonly AppenderDescriptor AspNetTrace,
                                                  Async,
                                                  BufferingForwarding,
                                                  Console,
                                                  Debug,
                                                  EventLog,
                                                  File,
                                                  Forwarding,
                                                  LocalSyslog,
                                                  ManagedColor,
                                                  Memory,
                                                  NetSend,
                                                  OutputDebugString,
                                                  RemoteSyslog,
                                                  Remoting,
                                                  RollingFile,
                                                  Smtp,
                                                  SmtpPickupDir,
                                                  Telnet,
                                                  TextWriter,
                                                  Trace,
                                                  Udp;

        /// <summary>
        /// All appender descriptors, ordered alphabetically by name.
        /// </summary>
        public static readonly IEnumerable<AppenderDescriptor> All;

        private static readonly IDictionary<string, AppenderDescriptor> sDescriptorsByTypeNamespace;

        static AppenderDescriptor()
        {
            AspNetTrace = new AppenderDescriptor("ASP.NET Trace", AppenderType.AspNetTrace, "log4net.Appender.AspNetTraceAppender");
            Async = new AppenderDescriptor("Async", AppenderType.Async, "Log4Net.Async.AsyncForwardingAppender,Log4Net.Async");
            BufferingForwarding = new AppenderDescriptor("Buffering Forwarding", AppenderType.BufferingForwarding, "log4net.Appender.BufferingForwardingAppender");
            Console = new AppenderDescriptor("Console", AppenderType.Console, "log4net.Appender.ConsoleAppender");
            Debug = new AppenderDescriptor("Debug", AppenderType.Debug, "log4net.Appender.DebugAppender");
            EventLog = new AppenderDescriptor("Event Log", AppenderType.EventLog, "log4net.Appender.EventLogAppender");
            File = new AppenderDescriptor("File", AppenderType.File, "log4net.Appender.FileAppender");
            Forwarding = new AppenderDescriptor("Forwarding", AppenderType.Forwarding, "log4net.Appender.ForwardingAppender");
            LocalSyslog = new AppenderDescriptor("Local Syslog", AppenderType.LocalSyslog, "log4net.Appender.LocalSyslogAppender");
            ManagedColor = new AppenderDescriptor("Managed Color", AppenderType.ManagedColor, "log4net.Appender.ManagedColoredConsoleAppender");
            Memory = new AppenderDescriptor("Memory", AppenderType.Memory, "log4net.Appender.MemoryAppender");
            NetSend = new AppenderDescriptor("Net Send", AppenderType.NetSend, "log4net.Appender.NetSendAppender");
            OutputDebugString = new AppenderDescriptor("Output Debug String", AppenderType.OutputDebugString, "log4net.Appender.OutputDebugStringAppender");
            RemoteSyslog = new AppenderDescriptor("Remote Syslog", AppenderType.RemoteSyslog, "log4net.Appender.RemoteSyslogAppender");
            Remoting = new AppenderDescriptor("Remoting", AppenderType.Remoting, "log4net.Appender.RemotingAppender");
            RollingFile = new AppenderDescriptor("Rolling File", AppenderType.RollingFile, "log4net.Appender.RollingFileAppender");
            Smtp = new AppenderDescriptor("SMTP", AppenderType.Smtp, "log4net.Appender.SmtpAppender");
            SmtpPickupDir = new AppenderDescriptor("SMTP Pickup Dir", AppenderType.SmtpPickupDir, "log4net.Appender.SmtpPickupDirAppender");
            Telnet = new AppenderDescriptor("Telnet", AppenderType.Telnet, "log4net.Appender.TelnetAppender");
            TextWriter = new AppenderDescriptor("Text Writer", AppenderType.TextWriter, "log4net.Appender.TextWriterAppender");
            Trace = new AppenderDescriptor("Trace", AppenderType.Trace, "log4net.Appender.TraceAppender");
            Udp = new AppenderDescriptor("UDP", AppenderType.Udp, "log4net.Appender.UdpAppender");

            All = new[]
            {
                AspNetTrace,
                Async,
                BufferingForwarding,
                Console,
                Debug,
                EventLog,
                File,
                Forwarding,
                LocalSyslog,
                ManagedColor,
                Memory,
                NetSend,
                OutputDebugString,
                RemoteSyslog,
                Remoting,
                RollingFile,
                Smtp,
                SmtpPickupDir,
                Telnet,
                TextWriter,
                Trace,
                Udp
            };

            sDescriptorsByTypeNamespace = new Dictionary<string, AppenderDescriptor>();

            foreach (AppenderDescriptor descriptor in All)
            {
                sDescriptorsByTypeNamespace.Add(descriptor.TypeNamespace, descriptor);
            }
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
