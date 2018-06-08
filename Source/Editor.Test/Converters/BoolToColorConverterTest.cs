// Copyright © 2018 Alex Leendertsen

using System;
using System.Windows.Media;
using Editor.Converters;
using NUnit.Framework;

namespace Editor.Test.Converters
{
    [TestFixture]
    public class BoolToColorConverterTest
    {
        private BoolToColorConverter mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new BoolToColorConverter();
        }

        [Test]
        public void Convert_ShouldReturnBlack_WhenNotABool()
        {
            Assert.AreEqual(Brushes.Black, mSut.Convert(new object(), null, null, null));
        }

        [Test]
        public void Convert_ShouldReturnRed_WhenTrue()
        {
            Assert.AreEqual(Brushes.Red, mSut.Convert(true, null, null, null));
        }

        [Test]
        public void Convert_ShouldReturnBlack_WhenFalse()
        {
            Assert.AreEqual(Brushes.Black, mSut.Convert(false, null, null, null));
        }

        [Test]
        public void ConvertBack_ShouldThrowNotSupported()
        {
            Assert.Throws<NotSupportedException>(() => mSut.ConvertBack(null, null, null, null));
        }
    }
}
