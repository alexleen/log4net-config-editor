// Copyright © 2019 Alex Leendertsen

using System;
using Editor.Converters;
using Editor.Enums;
using NUnit.Framework;

namespace Editor.Test.Converters
{
    [TestFixture]
    public class SaveStateToTextConverterTest
    {
        private SaveStateToTextConverter mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new SaveStateToTextConverter();
        }

        [TestCase(SaveState.Unknown, null)]
        [TestCase(SaveState.Changed, "Unsaved Changes")]
        [TestCase(SaveState.Saving, "Saving...")]
        [TestCase(SaveState.Saved, "All Changes Saved")]
        public void Convert_ShouldReturnCorrectValue(SaveState saveState, string expectedText)
        {
            Assert.AreEqual(expectedText, mSut.Convert(saveState, null, null, null));
        }

        [Test]
        public void ConvertBack_ShouldThrowNotSupportedException()
        {
            Assert.Throws<NotSupportedException>(() => mSut.ConvertBack(null, null, null, null));
        }
    }
}
