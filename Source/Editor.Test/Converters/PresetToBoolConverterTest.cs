// Copyright © 2018 Alex Leendertsen

using System;
using Editor.ConfigProperties;
using Editor.Converters;
using log4net.Core;
using NUnit.Framework;

namespace Editor.Test.Converters
{
    [TestFixture]
    public class PresetToBoolConverterTest
    {
        private PresetToBoolConverter mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new PresetToBoolConverter();
        }

        [TestCase(FixFlags.None, false)]
        [TestCase(Fix.NonePreset, false)]
        [TestCase(Fix.PartialPreset, false)]
        [TestCase(Fix.AllPreset, false)]
        [TestCase(Fix.CustomPreset, true)]
        public void Convert_ShouldConvertCorrectly(object value, bool expected)
        {
            Assert.AreEqual(expected, mSut.Convert(value, null, null, null));
        }

        [Test]
        public void ConvertBack_ShouldThrowNotSupported()
        {
            Assert.Throws<NotSupportedException>(() => mSut.ConvertBack(null, null, null, null));
        }
    }
}
