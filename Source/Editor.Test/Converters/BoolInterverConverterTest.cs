// Copyright © 2018 Alex Leendertsen

using Editor.Converters;
using NUnit.Framework;

namespace Editor.Test.Converters
{
    [TestFixture]
    public class BoolInterverConverterTest
    {
        private BoolInterverConverter mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new BoolInterverConverter();
        }

        [Test]
        public void Convert_ShouldReturnValue_WhenValueIsNotBool()
        {
            object value = new object();
            Assert.AreEqual(value, mSut.Convert(value, null, null, null));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Convert_ShouldInverse_WhenBool(bool value)
        {
            Assert.AreEqual(!value, mSut.Convert(value, null, null, null));
        }

        [Test]
        public void ConvertBack_ShouldReturnValue_WhenValueIsNotBool()
        {
            object value = new object();
            Assert.AreEqual(value, mSut.ConvertBack(value, null, null, null));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ConvertBack_ShouldInverse_WhenBool(bool value)
        {
            Assert.AreEqual(!value, mSut.ConvertBack(value, null, null, null));
        }
    }
}
