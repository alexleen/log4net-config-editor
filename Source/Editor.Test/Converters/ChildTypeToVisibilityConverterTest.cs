// Copyright © 2018 Alex Leendertsen

using System;
using System.Windows;
using Editor.Converters;
using Editor.Descriptors;
using Editor.Models.ConfigChildren;
using NUnit.Framework;

namespace Editor.Test.Converters
{
    [TestFixture]
    public class ChildTypeToVisibilityConverterTest
    {
        private ChildTypeToVisibilityConverter mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new ChildTypeToVisibilityConverter();
        }

        [Test]
        public void Convert_ShouldReturnHidden_WhenValueIsNotAppenderModel()
        {
            Assert.AreEqual(Visibility.Hidden, mSut.Convert(new object(), null, null, null));
        }

        [Test]
        public void Convert_ShouldReturnVisible_WhenAppenderModel()
        {
            Assert.AreEqual(Visibility.Visible, mSut.Convert(new AppenderModel(AppenderDescriptor.Async, null, 0), null, null, null));
        }

        [Test]
        public void ConvertBack_ShouldThrowNotSupportedException()
        {
            Assert.Throws<NotSupportedException>(() => mSut.ConvertBack(null, null, null, null));
        }
    }
}
