// Copyright © 2018 Alex Leendertsen

using System;
using System.Windows.Media;
using Editor.Converters;
using NUnit.Framework;

namespace Editor.Test.Converters
{
    [TestFixture]
    public class ConsoleColorToBrushConverterTest
    {
        private ConsoleColorToBrushConverter mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new ConsoleColorToBrushConverter();
        }

        [Test]
        public void Convert_ShouldReturnTransparent_WhenNotAConsoleColor()
        {
            Assert.AreEqual(Brushes.Transparent, mSut.Convert(new object(), null, null, null));
        }

        [Test]
        public void Convert_ShouldReturnCorrectColor()
        {
            Assert.AreEqual(Brushes.Black, mSut.Convert(ConsoleColor.Black, null, null, null));
        }

        [Test]
        public void Convert_ShouldHandleDarkYellow()
        {
            Assert.AreEqual(Brushes.DarkGoldenrod, mSut.Convert(ConsoleColor.DarkYellow, null, null, null));
        }

        [Test]
        public void ConvertBack_ShouldThrowNotSupported()
        {
            Assert.Throws<NotSupportedException>(() => mSut.ConvertBack(null, null, null, null));
        }
    }
}
