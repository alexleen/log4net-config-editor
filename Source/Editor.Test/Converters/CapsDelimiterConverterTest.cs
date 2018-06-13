// Copyright © 2018 Alex Leendertsen

using System;
using Editor.Converters;
using NUnit.Framework;

namespace Editor.Test.Converters
{
    [TestFixture]
    public class CapsDelimiterConverterTest
    {
        private CapsDelimiterConverter mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new CapsDelimiterConverter();
        }

        [TestCase("PascalCase", "Pascal Case")]
        [TestCase("camelCase", "camel Case")]
        public void Convert_ShouldConvertValueCorrectly(string value, string expected)
        {
            Assert.AreEqual(expected, mSut.Convert(value, null, null, null));
        }

        [Test]
        public void Convert_ShouldConvertNullToNull()
        {
            Assert.IsNull(mSut.Convert(null, null, null, null));
        }

        [Test]
        public void ConvertBack_ShouldThrowNotSupported()
        {
            Assert.Throws<NotSupportedException>(() => mSut.ConvertBack(null, null, null, null));
        }
    }
}
