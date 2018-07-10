// Copyright © 2018 Alex Leendertsen

using System;
using System.ComponentModel;
using Editor.Descriptors;
using Editor.Descriptors.Base;
using Editor.Enums;
using Editor.Interfaces;

namespace Editor.Windows.SizeLocation
{
    internal static class WindowSizeLocationFactory
    {
        internal static IWindowSizeLocation Create(DescriptorBase descriptor)
        {
            switch (descriptor)
            {
                case AppenderDescriptor appenderDescriptor:
                    return new AppenderWindowSizeLocation();
                case FilterDescriptor filterDescriptor:
                    return CreateFilterSizeLocation(filterDescriptor);
                case LoggerDescriptor loggerDescriptor:
                    return new LoggerWindowSizeLocation();
                case MappingDescriptor mappingDescriptor:
                    return new FilterWindowSizeLocation();
                case RendererDescriptor rendererDescriptor:
                    return new RendererWindowSizeLocation();
                default:
                    throw new ArgumentException($"Window size & locations do not exist for {descriptor.GetType().Name}");
            }
        }

        private static IWindowSizeLocation CreateFilterSizeLocation(FilterDescriptor filterDescriptor)
        {
            switch (filterDescriptor.Type)
            {
                case FilterType.LevelMatch:
                case FilterType.LevelRange:
                    return new FilterWindowSizeLocation();
                case FilterType.LoggerMatch:
                    return new LoggerMatchFilterWindowSizeLocation();
                case FilterType.Property:
                    return new PropertyMatchFilterWindowSizeLocation();
                case FilterType.String:
                    return new StringMatchFilterWindowSizeLocation();
                default:
                    throw new InvalidEnumArgumentException(nameof(filterDescriptor.Type), (int)filterDescriptor.Type, typeof(FilterType));
            }
        }
    }
}
