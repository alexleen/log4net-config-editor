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
    public class PropertyMatchFilterTest
    {
        private PropertyMatchFilter mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new PropertyMatchFilter();
        }

        [Test]
        public void Name_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("Property Match Filter", mSut.Name);
        }

        [Test]
        public void Icon_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("pack://application:,,,/Editor;component/Images/view-filter.png", mSut.Icon);
        }

        [Test]
        public void Descriptor_ShouldReturnCorrectValue()
        {
            Assert.AreEqual(FilterDescriptor.Property, mSut.Descriptor);
        }

        [Test]
        public void Initialize_ShouldAddTheCorrectNumberOfProperties()
        {
            mSut.Initialize();

            Assert.AreEqual(4, mSut.Properties.Count);
        }

        [Test]
        public void Initialize_ShouldAddStringProperties()
        {
            mSut.Initialize();

            mSut.Properties.Single(p => p.GetType() == typeof(BooleanPropertyBase));
            mSut.Properties.Single(p => p.GetType() == typeof(StringMatch));
            mSut.Properties.Single(p => p.GetType() == typeof(RegexMatch));
        }

        [Test]
        public void Initialize_ShouldAddKeyProperty()
        {
            mSut.Initialize();

            mSut.Properties.Single(p => p.GetType() == typeof(RequiredStringProperty));
        }

        [Test]
        public void Initialize_ShouldSetIsFocusedToTrue()
        {
            mSut.Initialize();

            RequiredStringProperty key = (RequiredStringProperty)mSut.Properties.Single(p => p.GetType() == typeof(RequiredStringProperty));

            Assert.IsTrue(key.IsFocused);
        }
    }
}
