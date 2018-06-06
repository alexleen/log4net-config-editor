// Copyright © 2018 Alex Leendertsen

using System;
using Editor.Converters;
using Editor.Enums;
using NUnit.Framework;

namespace Editor.Test.Converters
{
    [TestFixture]
    public class FilterTypeToBoolConverterTest
    {
        private FilterTypeToBoolConverter mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new FilterTypeToBoolConverter();
        }

        [Test]
        public void Convert_ShouldReturnFalse_WhenValueIsNotFilterModel()
        {
            object result = mSut.Convert(new object(), null, null, null);

            Assert.IsNotNull(result);
            Assert.IsFalse((bool)result);
        }

        [TestCase(FilterType.DenyAll, false)]
        [TestCase(FilterType.LevelMatch, true)]
        [TestCase(FilterType.LevelRange, true)]
        [TestCase(FilterType.LoggerMatch, true)]
        [TestCase(FilterType.Mdc, true)]
        [TestCase(FilterType.Ndc, true)]
        [TestCase(FilterType.Property, true)]
        [TestCase(FilterType.String, true)]
        public void Convert_ShouldReturnCorrectValue(FilterType filterType, bool expected)
        {
            Assert.AreEqual(expected, mSut.Convert(filterType, null, null, null));
        }

        [Test]
        public void ConvertBack_ShouldThrowNotSupportedException()
        {
            Assert.Throws<NotSupportedException>(() => mSut.ConvertBack(null, null, null, null));
        }
    }
}
