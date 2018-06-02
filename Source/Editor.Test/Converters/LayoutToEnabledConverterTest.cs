// Copyright © 2018 Alex Leendertsen

using System;
using Editor.Converters;
using Editor.Descriptors;
using NUnit.Framework;

namespace Editor.Test.Converters
{
    [TestFixture]
    public class LayoutToEnabledConverterTest
    {
        private LayoutToEnabledConverter mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new LayoutToEnabledConverter();
        }

        [Test]
        public void Convert_ShouldReturnNull_WhenValueIsNotLayoutDescriptor()
        {
            Assert.IsNull(mSut.Convert(new object(), null, null, null));
        }

        [Test]
        public void Convert_ShouldReturnCorrectValue()
        {
            object value = mSut.Convert(LayoutDescriptor.Simple, null, null, null);

            Assert.IsNotNull(value);
            Assert.IsFalse((bool)value);

            value = mSut.Convert(LayoutDescriptor.Pattern, null, null, null);

            Assert.IsNotNull(value);
            Assert.IsTrue((bool)value);
        }

        [Test]
        public void ConvertBack_ShouldThrowNotSupportedException()
        {
            Assert.Throws<NotSupportedException>(() => mSut.ConvertBack(null, null, null, null));
        }
    }
}
