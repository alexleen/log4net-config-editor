// Copyright © 2020 Alex Leendertsen

using System.Linq;
using Editor.ConfigProperties.Base;
using Editor.Definitions.Filters;
using Editor.Descriptors;
using NUnit.Framework;

namespace Editor.Test.Definitions.Filters
{
    [TestFixture]
    public class LevelRangeFilterTest
    {
        private LevelRangeFilter mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new LevelRangeFilter();
        }

        [Test]
        public void Name_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("Level Range Filter", mSut.Name);
        }

        [Test]
        public void Icon_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("pack://application:,,,/Editor;component/Images/view-filter.png", mSut.Icon);
        }

        [Test]
        public void Descriptor_ShouldReturnCorrectValue()
        {
            Assert.AreEqual(FilterDescriptor.LevelRange, mSut.Descriptor);
        }

        [Test]
        public void Initialize_ShouldAddTheCorrectNumberOfProperties()
        {
            mSut.Initialize();

            Assert.AreEqual(3, mSut.Properties.Count);
        }

        [Test]
        public void Initialize_ShouldAddDefaultProperties()
        {
            mSut.Initialize();

            mSut.Properties.Single(p => p is BooleanPropertyBase);
            mSut.Properties.Single(p => p is LevelPropertyBase lpb && lpb.Name == "Min Level:");
            mSut.Properties.Single(p => p is LevelPropertyBase lpb && lpb.Name == "Max Level:");
        }
    }
}
