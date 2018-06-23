// Copyright © 2018 Alex Leendertsen

using System;
using System.ComponentModel;
using System.Windows;
using Editor.Descriptors;
using Editor.Descriptors.Base;
using Editor.Enums;

namespace Editor.Interfaces
{
    internal interface IWindowSizeLocation
    {
        string RetentionKey { get; }

        ResizeMode ResizeMode { get; }

        SizeToContent SizeToContent { get; }

        WindowStartupLocation WindowStartupLocation { get; }

        Size Width { get; }

        Size Height { get; }
    }

    internal struct Size
    {
        public Size(double value, double max, double min)
        {
            Value = value;
            Max = max;
            Min = min;
        }

        public double Value { get; }
        public double Max { get; }
        public double Min { get; }
    }

    internal class AppenderWindowSizeLocation : IWindowSizeLocation
    {
        public string RetentionKey { get; } = "AppenderWindowPlacement";
        public ResizeMode ResizeMode { get; } = ResizeMode.CanResize;
        public SizeToContent SizeToContent { get; } = SizeToContent.Manual;
        public WindowStartupLocation WindowStartupLocation { get; } = WindowStartupLocation.CenterOwner;
        public Size Width { get; } = new Size(550, double.NaN, 550);
        public Size Height { get; } = new Size(500, double.NaN, 500);
    }

    internal class FilterWindowSizeLocation : IWindowSizeLocation
    {
        public string RetentionKey { get; } = null;
        public virtual ResizeMode ResizeMode { get; } = ResizeMode.NoResize;
        public SizeToContent SizeToContent { get; } = SizeToContent.WidthAndHeight;
        public WindowStartupLocation WindowStartupLocation { get; } = WindowStartupLocation.CenterOwner;
        public virtual Size Width { get; } = new Size(double.NaN, double.NaN, double.NaN);
        public virtual Size Height { get; } = new Size(double.NaN, double.NaN, double.NaN);
    }

    internal class LoggerMatchFilterWindowSizeLocation : FilterWindowSizeLocation
    {
        public override ResizeMode ResizeMode { get; } = ResizeMode.CanResize;
        public override Size Width { get; } = new Size(double.NaN, double.NaN, 350);
        public override Size Height { get; } = new Size(double.NaN, 121, 121);
    }

    internal class PropertyMatchFilterWindowSizeLocation : FilterWindowSizeLocation
    {
        public override ResizeMode ResizeMode { get; } = ResizeMode.CanResize;
        public override Size Width { get; } = new Size(double.NaN, double.NaN, 350);
        public override Size Height { get; } = new Size(double.NaN, 173, 173);
    }

    internal class StringMatchFilterWindowSizeLocation : FilterWindowSizeLocation
    {
        public override ResizeMode ResizeMode { get; } = ResizeMode.CanResize;
        public override Size Width { get; } = new Size(double.NaN, double.NaN, 350);
        public override Size Height { get; } = new Size(double.NaN, 147, 147);
    }

    internal class LoggerWindowSizeLocation : IWindowSizeLocation
    {
        public string RetentionKey { get; } = null;
        public ResizeMode ResizeMode { get; } = ResizeMode.CanResize;
        public SizeToContent SizeToContent { get; } = SizeToContent.Manual;
        public WindowStartupLocation WindowStartupLocation { get; } = WindowStartupLocation.CenterOwner;
        public Size Width { get; } = new Size(320, double.NaN, 320);
        public Size Height { get; } = new Size(280, double.NaN, 280);
    }

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
