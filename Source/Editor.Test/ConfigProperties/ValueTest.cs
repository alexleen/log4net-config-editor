// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class ValueTest
    {
        private Value mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new Value();
        }

        [TestCase(null, false)]
        [TestCase("", true)]
        public void Load_ShouldNotLoadValue(string value, bool retVal)
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("value", out _).Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns(value);
                    ci[1] = result;
                    return retVal;
                });

            mSut.Load(config);

            Assert.AreEqual(value, mSut.Value);
        }

        [Test]
        public void Load_ShouldLoadCorrectValue()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("value", out _).Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns("log4net.Appender.ColoredConsoleAppender");
                    ci[1] = result;
                    return true;
                });

            mSut.Load(config);

            Assert.AreEqual("log4net.Appender.ColoredConsoleAppender", mSut.Value);
        }

        [TestCase(null)]
        [TestCase("")]
        public void Save_ShouldNotSaveValueToAttribute_WhenValueIsNullOrEmpty(string value)
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.Value = value;

            mSut.Save(config);

            config.DidNotReceive().Save(Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public void Name_ShouldBeCorrect_RegularCtor()
        {
            Assert.AreEqual("Value:", mSut.Name);
        }

        [Test]
        public void Save_ShouldSaveValueToAttribute()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            const string value = "whatev";
            mSut.Value = value;

            mSut.Save(config);

            config.Received(1).Save("value", value);
        }

        [Test]
        public void Value_ShouldBeCorrect_RegularCtor()
        {
            Assert.IsNull(mSut.Value);
        }
    }
}
