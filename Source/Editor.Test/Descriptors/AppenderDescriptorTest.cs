// Copyright © 2020 Alex Leendertsen

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Editor.Descriptors;
using Editor.Enums;
using NUnit.Framework;

namespace Editor.Test.Descriptors
{
    [TestFixture]
    internal class AppenderDescriptorTest
    {
        private FieldInfo[] mAppenders;

        [SetUp]
        public void SetUp()
        {
            mAppenders = typeof(AppenderDescriptor).GetFields(BindingFlags.Public | BindingFlags.Static);
        }

        private static readonly IEnumerable<TestCaseData> sAppenderData = new[]
        {
            new TestCaseData("ASP.NET Trace", AppenderType.AspNetTrace, "log4net.Appender.AspNetTraceAppender", "appender"),
            new TestCaseData("Async", AppenderType.Async, "Log4Net.Async.AsyncForwardingAppender,Log4Net.Async", "appender"),
            new TestCaseData("AWS Appender", AppenderType.AwsAppender, "AWS.Logger.Log4net.AWSAppender,AWS.Logger.Log4net", "appender"),
            new TestCaseData("Buffering Forwarding", AppenderType.BufferingForwarding, "log4net.Appender.BufferingForwardingAppender", "appender"),
            new TestCaseData("Console", AppenderType.Console, "log4net.Appender.ConsoleAppender", "appender"),
            new TestCaseData("Debug", AppenderType.Debug, "log4net.Appender.DebugAppender", "appender"),
            new TestCaseData("Event Log", AppenderType.EventLog, "log4net.Appender.EventLogAppender", "appender"),
            new TestCaseData("File", AppenderType.File, "log4net.Appender.FileAppender", "appender"),
            new TestCaseData("Forwarding", AppenderType.Forwarding, "log4net.Appender.ForwardingAppender", "appender"),
            new TestCaseData("Local Syslog", AppenderType.LocalSyslog, "log4net.Appender.LocalSyslogAppender", "appender"),
            new TestCaseData("Managed Color", AppenderType.ManagedColor, "log4net.Appender.ManagedColoredConsoleAppender", "appender"),
            new TestCaseData("Memory", AppenderType.Memory, "log4net.Appender.MemoryAppender", "appender"),
            new TestCaseData("Net Send", AppenderType.NetSend, "log4net.Appender.NetSendAppender", "appender"),
            new TestCaseData("Output Debug String", AppenderType.OutputDebugString, "log4net.Appender.OutputDebugStringAppender", "appender"),
            new TestCaseData("Remote Syslog", AppenderType.RemoteSyslog, "log4net.Appender.RemoteSyslogAppender", "appender"),
            new TestCaseData("Remoting", AppenderType.Remoting, "log4net.Appender.RemotingAppender", "appender"),
            new TestCaseData("Rolling File", AppenderType.RollingFile, "log4net.Appender.RollingFileAppender", "appender"),
            new TestCaseData("SMTP Pickup Dir", AppenderType.SmtpPickupDir, "log4net.Appender.SmtpPickupDirAppender", "appender"),
            new TestCaseData("SMTP", AppenderType.Smtp, "log4net.Appender.SmtpAppender", "appender"),
            new TestCaseData("Telnet", AppenderType.Telnet, "log4net.Appender.TelnetAppender", "appender"),
            new TestCaseData("Text Writer", AppenderType.TextWriter, "log4net.Appender.TextWriterAppender", "appender"),
            new TestCaseData("Trace", AppenderType.Trace, "log4net.Appender.TraceAppender", "appender"),
            new TestCaseData("UDP", AppenderType.Udp, "log4net.Appender.UdpAppender", "appender")
        };

        [TestCaseSource(nameof(sAppenderData))]
        public void EachAppender_ShouldHaveCorrectAppenderField(string name, AppenderType type, string typeNamespace, string elementName)
        {
            mAppenders.Single(a => AreEqual((AppenderDescriptor)a.GetValue(null), name, type, typeNamespace, elementName));
        }

        [Test]
        public void EachAppender_ShouldBeTested()
        {
            Assert.AreEqual(sAppenderData.Count(), mAppenders.Length);
        }

        [Test]
        public void EachAppender_ShouldHaveTestData()
        {
            foreach (FieldInfo info in mAppenders)
            {
                sAppenderData.Single(f => AreEqual((AppenderDescriptor)info.GetValue(null), (string)f.Arguments[0], (AppenderType)f.Arguments[1], (string)f.Arguments[2], (string)f.Arguments[3]));
            }
        }

        [TestCaseSource(nameof(sAppenderData))]
        public void EachAppender_ShouldBeFoundByTypeNamespace(string name, AppenderType type, string typeNamespace, string elementName)
        {
            Assert.IsTrue(AppenderDescriptor.TryFindByTypeNamespace(typeNamespace, out AppenderDescriptor appender));
            Assert.IsTrue(AreEqual(appender, name, type, typeNamespace, elementName));
        }

        private bool AreEqual(AppenderDescriptor descriptor, string name, AppenderType type, string typeNamespace, string elementName)
        {
            return descriptor.Name == name &&
                   descriptor.Type == type &&
                   descriptor.TypeNamespace == typeNamespace &&
                   descriptor.ElementName == elementName;
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("whatev")]
        public void TryFindByTypeNamespace_ShouldReturnFalse_AndNull_ForInvalidTypeNamespace(string typeNamespace)
        {
            Assert.IsFalse(AppenderDescriptor.TryFindByTypeNamespace(typeNamespace, out AppenderDescriptor descriptor));
            Assert.IsNull(descriptor);
        }
    }
}
