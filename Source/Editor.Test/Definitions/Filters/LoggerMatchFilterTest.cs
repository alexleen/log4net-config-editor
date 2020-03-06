// Copyright © 2018 Alex Leendertsen

using System.Linq;
using Editor.ConfigProperties;
using Editor.ConfigProperties.Base;
using Editor.Definitions.Filters;
using Editor.Descriptors;
using NUnit.Framework;

namespace Editor.Test.Definitions.Filters
{
    [TestFixture]
    public class LoggerMatchFilterTest
    {
        private LoggerMatchFilter mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new LoggerMatchFilter();
        }

        [Test]
        public void Name_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("Logger Match Filter", mSut.Name);
        }

        [Test]
        public void Icon_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("pack://application:,,,/Editor;component/Images/view-filter.png", mSut.Icon);
        }

        [Test]
        public void Descriptor_ShouldReturnCorrectValue()
        {
            Assert.AreEqual(FilterDescriptor.LoggerMatch, mSut.Descriptor);
        }

        [Test]
        public void Initialize_ShouldAddTheCorrectNumberOfProperties()
        {
            mSut.Initialize();

            Assert.AreEqual(2, mSut.Properties.Count);
        }

        [Test]
        public void Initialize_ShouldAddDefaultProperties()
        {
            mSut.Initialize();

            mSut.Properties.Single(p => p.GetType() == typeof(BooleanPropertyBase));
            mSut.Properties.Single(p => p.GetType() == typeof(RequiredStringProperty));
        }

        [Test]
        public void Initialize_ShouldSetIsFocusedToTrue()
        {
            mSut.Initialize();

            RequiredStringProperty loggerToMatch = (RequiredStringProperty)mSut.Properties.Single(p => p.GetType() == typeof(RequiredStringProperty));

            Assert.IsTrue(loggerToMatch.IsFocused);
        }
    }
}
