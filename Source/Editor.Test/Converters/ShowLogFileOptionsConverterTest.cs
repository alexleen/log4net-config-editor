// Copyright © 2019 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Windows;
using Editor.Converters;
using Editor.Descriptors;
using Editor.Models.ConfigChildren;
using NUnit.Framework;

namespace Editor.Test.Converters
{
    [TestFixture]
    public class ShowLogFileOptionsConverterTest
    {
        private ShowLogFileOptionsConverter mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new ShowLogFileOptionsConverter();
        }

        private static readonly IEnumerable<TestCaseData> sVisibleOptions = new[]
        {
            new TestCaseData(new AppenderModel(AppenderDescriptor.File, null, 0), "file"),
            new TestCaseData(new AppenderModel(AppenderDescriptor.File, null, 0), "dir"),
            new TestCaseData(new AppenderModel(AppenderDescriptor.RollingFile, null, 0), "dir")
        };

        [TestCaseSource(nameof(sVisibleOptions))]
        public void Convert_ShouldReturnVisible(AppenderModel model, string param)
        {
            Assert.AreEqual(Visibility.Visible, mSut.Convert(model, null, param, null));
        }

        private static readonly IEnumerable<TestCaseData> sInvalidOptions = new[]
        {
            new TestCaseData(null, null),
            new TestCaseData(null, "file"),
            new TestCaseData(new AppenderModel(AppenderDescriptor.File, null, 0), null),
            new TestCaseData(new AppenderModel(AppenderDescriptor.File, null, 0), "whatev"),
            new TestCaseData(new AppenderModel(AppenderDescriptor.Async, null, 0), "file"),
            new TestCaseData(new AppenderModel(AppenderDescriptor.Async, null, 0), "dir")
        };

        [TestCaseSource(nameof(sInvalidOptions))]
        public void Convert_ShouldReturnHidden_WhenInvalidInputs(object model, object parameter)
        {
            Assert.AreEqual(Visibility.Hidden, mSut.Convert(model, null, parameter, null));
        }

        [Test]
        public void ConvertBack_ShouldThrowNotSupportedException()
        {
            Assert.Throws<NotSupportedException>(() => mSut.ConvertBack(null, null, null, null));
        }
    }
}
