using Editor.Descriptors;
using Editor.Descriptors.Base;
using Editor.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Editor.Test.Windows.SizeLocation
{
    [TestFixture]
    public class WindowSizeLocationFactoryTest
    {
        private static readonly IEnumerable<TestCaseData> DescriptorCases = new[]
        {
            //Appenders
            new TestCaseData(AppenderDescriptor.Console, "AppenderWindowPlacement", ResizeMode.CanResize, SizeToContent.Manual, new Interfaces.Size(550, min: 550), new Interfaces.Size(500, min: 500)),

            //Filters
            new TestCaseData(FilterDescriptor.LevelMatch, null, ResizeMode.NoResize, SizeToContent.WidthAndHeight, new Interfaces.Size(), new Interfaces.Size()),
            new TestCaseData(FilterDescriptor.LevelRange, null, ResizeMode.NoResize, SizeToContent.WidthAndHeight, new Interfaces.Size(), new Interfaces.Size()),
            new TestCaseData(FilterDescriptor.LoggerMatch, null, ResizeMode.CanResize, SizeToContent.WidthAndHeight, new Interfaces.Size(min: 350), new Interfaces.Size(min: 121, max: 121)),
            new TestCaseData(FilterDescriptor.Property, null, ResizeMode.CanResize, SizeToContent.WidthAndHeight, new Interfaces.Size(min: 350), new Interfaces.Size(min: 173, max: 173)),
            new TestCaseData(FilterDescriptor.String, null, ResizeMode.CanResize, SizeToContent.WidthAndHeight, new Interfaces.Size(min: 350), new Interfaces.Size(min: 147, max: 147)),

            //Loggers
            new TestCaseData(LoggerDescriptor.Root, null, ResizeMode.CanResize, SizeToContent.Manual, new Interfaces.Size(350, min: 350), new Interfaces.Size(350, min: 350)),

            //Maping
            new TestCaseData(MappingDescriptor.Mapping, null, ResizeMode.NoResize, SizeToContent.WidthAndHeight, new Interfaces.Size(), new Interfaces.Size())
        };

        [TestCaseSource(nameof(DescriptorCases))]
        public void Create_ShouldCreateTheCorrectSizeLocation(DescriptorBase descriptorBase, string retentionKey, ResizeMode mode, SizeToContent sizeToContent, Interfaces.Size width, Interfaces.Size height)
        {
            IWindowSizeLocation windowSizeLocation = WindowSizeLocationFactory.Create(descriptorBase);

            Assert.AreEqual(retentionKey, windowSizeLocation.RetentionKey);
            Assert.AreEqual(mode, windowSizeLocation.ResizeMode);
            Assert.AreEqual(sizeToContent, windowSizeLocation.SizeToContent);
            Assert.AreEqual(width, windowSizeLocation.Width);
            Assert.AreEqual(height, windowSizeLocation.Height);
        }

        [Test]
        public void Create_ShouldThrow_ForUnknownDescriptor()
        {
            Assert.Throws<ArgumentException>(() => WindowSizeLocationFactory.Create(LayoutDescriptor.Simple));
        }

        [Test]
        public void Create_ShouldThrow_ForDenyAll()
        {
            Assert.Throws<InvalidEnumArgumentException>(() => WindowSizeLocationFactory.Create(FilterDescriptor.DenyAll));
        }
    }
}
