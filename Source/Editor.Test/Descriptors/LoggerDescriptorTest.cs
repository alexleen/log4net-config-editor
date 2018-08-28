// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors;
using Editor.Enums;
using NUnit.Framework;

namespace Editor.Test.Descriptors
{
    [TestFixture]
    public class LoggerDescriptorTest
    {
        [Test]
        public void Root_ShouldHaveCorrectName()
        {
            Assert.AreEqual("Root", LoggerDescriptor.Root.Name);
        }

        [Test]
        public void Root_ShouldHaveCorrectType()
        {
            Assert.AreEqual(LoggerType.Root, LoggerDescriptor.Root.LoggerType);
        }

        [Test]
        public void Logger_ShouldHaveCorrectName()
        {
            Assert.AreEqual("Logger", LoggerDescriptor.Logger.Name);
        }

        [Test]
        public void Logger_ShouldHaveCorrectType()
        {
            Assert.AreEqual(LoggerType.Logger, LoggerDescriptor.Logger.LoggerType);
        }
    }
}
