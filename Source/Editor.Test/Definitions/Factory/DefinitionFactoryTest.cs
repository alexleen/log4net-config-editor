// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Editor.Definitions.Appenders;
using Editor.Definitions.Factory;
using Editor.Definitions.Filters;
using Editor.Definitions.Loggers;
using Editor.Definitions.Mapping;
using Editor.Descriptors;
using Editor.Descriptors.Base;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.Definitions.Factory
{
    [TestFixture]
    public class DefinitionFactoryTest
    {
        private static IEnumerable<TestCaseData> DescriptorDefinitions => new[]
        {
            //Appenders
            new TestCaseData(AppenderDescriptor.Console, typeof(ConsoleAppender)),
            new TestCaseData(AppenderDescriptor.File, typeof(FileAppender)),
            new TestCaseData(AppenderDescriptor.RollingFile, typeof(RollingFileAppender)),
            new TestCaseData(AppenderDescriptor.EventLog, typeof(EventLogAppender)),
            new TestCaseData(AppenderDescriptor.Async, typeof(AsyncAppender)),
            new TestCaseData(AppenderDescriptor.Forwarding, typeof(ForwardingAppender)),
            new TestCaseData(AppenderDescriptor.ManagedColor, typeof(ManagedColoredConsoleAppender)),

            //Filters
            new TestCaseData(FilterDescriptor.LevelMatch, typeof(LevelMatchFilter)),
            new TestCaseData(FilterDescriptor.LevelRange, typeof(LevelRangeFilter)),
            new TestCaseData(FilterDescriptor.LoggerMatch, typeof(LoggerMatchFilter)),
            new TestCaseData(FilterDescriptor.Property, typeof(PropertyMatchFilter)),
            new TestCaseData(FilterDescriptor.String, typeof(StringMatchFilter)),

            //Logger
            new TestCaseData(LoggerDescriptor.Root, typeof(RootLogger)),
            new TestCaseData(LoggerDescriptor.Logger, typeof(Logger)),

            //Mapping
            new TestCaseData(MappingDescriptor.Mapping, typeof(MappingDefinition))
        };

        [TestCaseSource(nameof(DescriptorDefinitions))]
        public void Create_ShouldReturnTheCorrectDefinition(DescriptorBase descriptorBase, Type expectedDefinitionType)
        {
            Assert.IsInstanceOf(expectedDefinitionType, DefinitionFactory.Create(descriptorBase, Substitute.For<IElementConfiguration>()));
        }

        [Test]
        public void Create_ShouldThrow_WhenUnknownDescriptor()
        {
            Assert.Throws<ArgumentException>(() => DefinitionFactory.Create(LayoutDescriptor.Simple, Substitute.For<IElementConfiguration>()));
        }

        [Test]
        public void Create_ShouldThrow_WhenDenyAll()
        {
            Assert.Throws<InvalidEnumArgumentException>(() => DefinitionFactory.Create(FilterDescriptor.DenyAll, Substitute.For<IElementConfiguration>()));
        }
    }
}
