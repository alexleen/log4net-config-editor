// Copyright © 2020 Alex Leendertsen

using System;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties.Base
{
    [TestFixture]
    public class BooleanPropertyBaseTest
    {
        private string mName;
        private bool mDefaultValue;
        private BooleanPropertyBase mSut;

        [SetUp]
        public void SetUp()
        {
            mDefaultValue = true;
            mName = "name";
            mSut = new BooleanPropertyBase(mName, "elementName", mDefaultValue);
        }

        [Test]
        public void Load_ShouldNotLoad()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load(Arg.Any<string>(), out _, Arg.Any<string>()).Returns(false);

            mSut.Load(config);

            Assert.AreEqual(mDefaultValue, mSut.Value);
        }

        [TestCase("", true)]
        [TestCase("whatev", true)]
        [TestCase("False", false)]
        [TestCase("false", false)]
        public void Load_ShouldLoadTheCorrectValue(string value, bool expected)
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("value", out _, "elementName").Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns(value);
                    ci[1] = result;
                    return true;
                });

            mSut.Load(config);

            Assert.AreEqual(expected, mSut.Value);
        }

        [Test]
        public void Ctor_ShouldAssignName()
        {
            Assert.AreEqual(mName, mSut.Name);
        }

        [Test]
        public void Ctor_ShouldAssignValueToDefault()
        {
            Assert.AreEqual(mDefaultValue, mSut.Value);
        }

        [Test]
        public void Ctor_ShouldThrowIfElementNameIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BooleanPropertyBase("name", null, true));
        }

        [Test]
        public void Ctor_ShouldThrowIfNameIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BooleanPropertyBase(null, "elementName", true));
        }

        [Test]
        public void Save_ShouldIgnoreDefaultValue()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.Save(config);

            config.DidNotReceive().Save(Arg.Any<(string ElementName, string AttributeName, string AttributeValue)[]>());
        }

        [Test]
        public void Save_ShouldSaveNonDefaultValue()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.Value = !mDefaultValue;
            mSut.Save(config);

            config.Received(1).Save(("elementName", "value", mSut.Value.ToString()));
        }
    }
}
