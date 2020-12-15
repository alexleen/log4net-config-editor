// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Interfaces;
using Editor.XML;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class CountDirectionTest
    {
        private const string Lower = "Lower";
        private const string Higher = "Higher";
        private const string CountDirectionName = "countDirection";
        private CountDirection mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new CountDirection();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("string")]
        public void Load_ShouldSetDefaultValue_WhenAttributeValueIsNotAnInt(string value)
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("value", out _, CountDirectionName).Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns(value);
                    ci[1] = result;
                    return true;
                });

            mSut.Load(config);

            Assert.AreEqual(Lower, mSut.SelectedDirection);
        }

        [TestCase(-1, Lower)]
        [TestCase(0, Higher)]
        [TestCase(1, Higher)]
        public void Load_ShouldSetCorrectValue(int directionInt, string directionStr)
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("value", out _, CountDirectionName).Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns(directionInt.ToString());
                    ci[1] = result;
                    return true;
                });

            mSut.Load(config);

            Assert.AreEqual(directionStr, mSut.SelectedDirection);
        }

        [Test]
        public void Ctor_ShouldInitDefaultToLower()
        {
            Assert.AreEqual(Lower, mSut.SelectedDirection);
        }

        [Test]
        public void Ctor_ShouldInitDirectionsCorrectly()
        {
            CollectionAssert.AreEquivalent(new[] { Lower, Higher }, mSut.Directions);
        }

        [Test]
        public void Save_ShouldNotSaveIfNotHigher()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.Save(config);

            config.DidNotReceiveWithAnyArgs().Save();
        }

        [Test]
        public void Save_ShouldSaveIfHigher()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.SelectedDirection = Higher;

            mSut.Save(config);

            config.Received(1).Save(new Element(CountDirectionName, new[] { ("value", "0") }));
        }
    }
}
