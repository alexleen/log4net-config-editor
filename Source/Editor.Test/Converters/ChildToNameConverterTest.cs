// Copyright © 2018 Alex Leendertsen

using System;
using Editor.Converters;
using Editor.Descriptors;
using Editor.Models;
using NUnit.Framework;

namespace Editor.Test.Converters
{
    [TestFixture]
    public class ChildToNameConverterTest
    {
        private ChildToNameConverter mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new ChildToNameConverter();
        }

        [Test]
        public void Convert_ShouldReturnNull_WhenValueIsNotChildModel()
        {
            Assert.IsNull(mSut.Convert(new object(), null, null, null));
        }

        [Test]
        public void Convert_ShouldReturnElementName_WhenChildModel()
        {
            const string elementName = "root";
            Assert.AreEqual(elementName, mSut.Convert(new ChildModel(elementName, null), null, null, null));
        }

        [Test]
        public void Convert_ShouldReturnName_WhenAppenderModel()
        {
            const string name = "root";
            Assert.AreEqual(name, mSut.Convert(new AppenderModel(AppenderDescriptor.Async, null, name, 0), null, null, null));
        }

        [Test]
        public void ConvertBack_ShouldThrowNotSupportedException()
        {
            Assert.Throws<NotSupportedException>(() => mSut.ConvertBack(null, null, null, null));
        }
    }
}
