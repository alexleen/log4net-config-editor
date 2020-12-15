// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class TypeAttributeTest
    {
        [SetUp]
        public void SetUp()
        {
            mSut = new TypeAttribute(AppenderDescriptor.Async);
        }

        private TypeAttribute mSut;

        [TestCase(null)]
        [TestCase("")]
        public void Load_ShouldNotLoadType_RegularCtor(string value)
        {
            mSut = new TypeAttribute();

            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("value", out _, "category", "conversionPattern").Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns(value);
                    ci[1] = result;
                    return false;
                });

            mSut.Load(config);

            Assert.IsNull(mSut.Value);
        }

        [TestCase(null)]
        [TestCase("")]
        public void Load_ShouldMaintainType_FromCtor(string type)
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("value", out _, "category", "conversionPattern").Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns(type);
                    ci[1] = result;
                    return false;
                });

            mSut.Load(config);

            Assert.AreEqual(AppenderDescriptor.Async.TypeNamespace, mSut.Value);
        }

        [TestCase(null)]
        [TestCase("")]
        public void Save_ShouldNotSaveValueToAttribute_WhenValueIsNullOrEmpty(string value)
        {
            mSut.Value = value;

            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            mSut.Save(config);

            config.DidNotReceiveWithAnyArgs().Save();
        }

        [Test]
        public void IsReadOnly_ShouldBeFalse_RegularCtor()
        {
            mSut = new TypeAttribute();

            Assert.IsFalse(mSut.IsReadOnly);
        }

        [Test]
        public void IsReadOnly_ShouldBeTrue_AppenderDescriptorCtor()
        {
            Assert.IsTrue(mSut.IsReadOnly);
        }

        [Test]
        public void Load_ShouldLoadCorrectType()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.Load(config);

            config.Received(1).Load("type", out _);
        }

        [Test]
        public void Name_ShouldBeCorrect_AppenderDescriptorCtor()
        {
            Assert.AreEqual("Type:", mSut.Name);
        }

        [Test]
        public void Name_ShouldBeCorrect_RegularCtor()
        {
            mSut = new TypeAttribute();

            Assert.AreEqual("Type:", mSut.Name);
        }

        [Test]
        public void Save_ShouldSaveValueToAttribute()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            const string value = "type";
            mSut.Value = value;

            mSut.Save(config);

            config.Received(1).Save("type", value);
        }

        [Test]
        public void Value_ShouldBeCorrect_AppenderDescriptorCtor()
        {
            Assert.AreEqual(AppenderDescriptor.Async.TypeNamespace, mSut.Value);
        }

        [Test]
        public void Value_ShouldBeCorrect_RegularCtor()
        {
            mSut = new TypeAttribute();

            Assert.IsNull(mSut.Value);
        }
    }
}
