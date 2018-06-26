// Copyright © 2018 Alex Leendertsen

using Editor.Definitions.Appenders;
using Editor.Definitions.Filters;
using Editor.Definitions.Loggers;
using Editor.Definitions.Mapping;
using Editor.Descriptors;
using Editor.Descriptors.Base;
using Editor.Enums;
using Editor.Interfaces;
using System;
using System.ComponentModel;

namespace Editor.Definitions.Factory
{
    public static class DefinitionFactory
    {
        public static IPropertyDefinition Create(DescriptorBase descriptor, IElementConfiguration configuration)
        {
            switch (descriptor)
            {
                case AppenderDescriptor appenderDescriptor:
                    return CreateAppenderDefinition(appenderDescriptor, configuration);
                case FilterDescriptor filterDescriptor:
                    return CreateFilterDefinition(filterDescriptor);
                case LoggerDescriptor loggerDescriptor:
                    return new RootLogger(configuration);
                case MappingDescriptor mappingDescriptor:
                    return new MappingDefinition();
                default:
                    throw new ArgumentException($"Property definitions do not exist for {descriptor.GetType().Name}");
            }
        }

        private static IPropertyDefinition CreateAppenderDefinition(AppenderDescriptor appenderDescriptor, IElementConfiguration appenderConfiguration)
        {
            switch (appenderDescriptor.Type)
            {
                case AppenderType.Console:
                    return new ConsoleAppender(appenderConfiguration);
                case AppenderType.File:
                    return new FileAppender(appenderConfiguration);
                case AppenderType.RollingFile:
                    return new RollingFileAppender(appenderConfiguration);
                case AppenderType.EventLog:
                    return new EventLogAppender(appenderConfiguration);
                case AppenderType.Async:
                    return new AsyncAppender(appenderConfiguration);
                case AppenderType.Forwarding:
                    return new ForwardingAppender(appenderConfiguration);
                case AppenderType.ManagedColor:
                    return new ManagedColoredConsoleAppender(appenderConfiguration);
                default:
                    throw new InvalidEnumArgumentException(nameof(appenderDescriptor.Type), (int)appenderDescriptor.Type, typeof(AppenderType));
            }
        }

        private static IPropertyDefinition CreateFilterDefinition(FilterDescriptor filterDescriptor)
        {
            switch (filterDescriptor.Type)
            {
                case FilterType.LevelMatch:
                    return new LevelMatchFilter();
                case FilterType.LevelRange:
                    return new LevelRangeFilter();
                case FilterType.LoggerMatch:
                    return new LoggerMatchFilter();
                case FilterType.Property:
                    return new PropertyMatchFilter();
                case FilterType.String:
                    return new StringMatchFilter();
                default:
                    throw new InvalidEnumArgumentException(nameof(filterDescriptor.Type), (int)filterDescriptor.Type, typeof(FilterType));
            }
        }
    }
}
