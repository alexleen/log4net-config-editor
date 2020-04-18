// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class AditivityTest
    {
        private Aditivity mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new Aditivity();
        }

        [TestCase(null, false)]
        [TestCase("", true)]
        public void Load_ShouldNotLoadValue(string value, bool retVal)
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("aditivity", out _).Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns(value);
                    ci[1] = result;
                    return retVal;
                });

            mSut.Load(config);

            Assert.IsTrue(mSut.Value);
        }

        [TestCase("FALSE", false)]
        [TestCase("False", false)]
        [TestCase("false", false)]
        public void Load_ShouldLoadTheCorrectValue(string value, bool expected)
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("aditivity", out _).Returns(ci =>
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
        public void Name_ShouldBeInitializedCorrectly()
        {
            Assert.AreEqual("Aditivity:", mSut.Name);
        }

        [Test]
        public void Save_ShouldNotSaveIfAditive()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.Save(config);

            config.DidNotReceive().Save(Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public void Save_ShouldSaveIfNotAditive()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.Value = false;
            mSut.Save(config);

            config.Received(1).Save("aditivity", "False");
        }

        [Test]
        public void Value_ShouldDefaultToTrue()
        {
            Assert.IsTrue(mSut.Value);
        }
    }
}
