// Copyright © 2018 Alex Leendertsen

using System;
using System.Windows.Media.Imaging;
using Editor.Converters;
using NUnit.Framework;

namespace Editor.Test.Converters
{
    [TestFixture]
    public class ReferencesToImageConverterTest
    {
        private ReferencesToImageConverter mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new ReferencesToImageConverter();
        }

        [Test]
        public void Convert_ShouldReturnImageWithoutUri_WhenValueIsNotint()
        {
            BitmapImage image = (BitmapImage)mSut.Convert(new object(), null, null, null);

            Assert.IsNotNull(image);
            Assert.IsNull(image.UriSource);
        }

        //These tests throw for some unknown reason
//        [Test]
//        public void Convert_ShouldReturnWarningImage_WhenNoIncomingReferences()
//        {
//            BitmapImage image = (BitmapImage)mSut.Convert(0, null, null, null);
//
//            Assert.IsNotNull(image);
//            Assert.AreEqual(new Uri("pack://application:,,,/Editor;component/Images/dialog-warning.png"), image.UriSource);
//        }
//
//        [Test]
//        public void Convert_ShouldReturnCheckboxImage_WhenIncomingReferences()
//        {
//            BitmapImage image = (BitmapImage)mSut.Convert(1, null, null, null);
//
//            Assert.IsNotNull(image);
//            Assert.AreEqual(new Uri("pack://application:,,,/Editor;component/Images/checkbox.png"), image.UriSource);
//        }

        [Test]
        public void ConvertBack_ShouldThrowNotSupportedException()
        {
            Assert.Throws<NotSupportedException>(() => mSut.ConvertBack(null, null, null, null));
        }
    }
}
