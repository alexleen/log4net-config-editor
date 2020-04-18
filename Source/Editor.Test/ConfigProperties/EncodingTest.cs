// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class EncodingTest
    {
        [SetUp]
        public void SetUp()
        {
            mSut = new Encoding("Encoding:", "encoding");
        }

        private Encoding mSut;

        [TestCase("", "")]
        [TestCase("us-ascii", "us-ascii")]
        [TestCase("utf-16", "utf-16")]
        [TestCase("utf-16BE", "utf-16BE")]
        [TestCase("utf-7", "utf-7")]
        [TestCase("utf-8", "utf-8")]
        [TestCase("utf-32", "utf-32")]
        [TestCase("unknown", "")]
        public void Load_ShouldLoadCorrectValue(string value, string expected)
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("value", out _, "encoding").Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns(value);
                    ci[1] = result;
                    return true;
                });

            mSut.Load(config);

            Assert.AreEqual(expected, mSut.SelectedValue);
        }

        [Test]
        public void Ctor_ShouldInitializeSelectedValueToEmpty()
        {
            Assert.AreEqual(string.Empty, mSut.SelectedValue);
        }

        [Test]
        public void Save_ShouldNotSaveSelectedValue_WhenNotSelected()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.Save(config);

            config.DidNotReceive().Save(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public void Save_ShouldSaveSelectedValue_WhenSelected()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.SelectedValue = "whatev";
            mSut.Save(config);

            config.Received(1).Save("encoding", "value", mSut.SelectedValue);
        }
    }
}
