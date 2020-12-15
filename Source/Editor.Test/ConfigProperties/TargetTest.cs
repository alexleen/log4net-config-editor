// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Interfaces;
using Editor.XML;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class TargetTest
    {
        [SetUp]
        public void SetUp()
        {
            mSut = new Target();
        }

        private const string ConsoleOut = "Console.Out";
        private const string ConsoleError = "Console.Error";
        private Target mSut;

        [TestCase(ConsoleOut)]
        [TestCase(ConsoleError)]
        public void Load_ShouldLoadCorrectly(string expected)
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("value", out _, "target").Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns(expected);
                    ci[1] = result;
                    return true;
                });

            mSut.Load(config);

            Assert.AreEqual(expected, mSut.SelectedItem);
        }

        [Test]
        public void Save_ShouldNotSaveIfConsoleOut()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.Save(config);

            config.DidNotReceive().Save(new Element("target", new[] { ("value", ConsoleError) }));
        }

        [Test]
        public void Save_ShouldSaveIfConsoleError()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.SelectedItem = ConsoleError;
            mSut.Save(config);

            config.Received(1).Save(new Element("target", new[] { ("value", ConsoleError) }));
        }

        [Test]
        public void SelectedTarget_ShouldBeInitializedToNone()
        {
            Assert.AreEqual(ConsoleOut, mSut.SelectedItem);
        }

        [Test]
        public void Targets_ShouldBeInitializedCorrectly()
        {
            CollectionAssert.AreEqual(new[] { ConsoleOut, ConsoleError }, mSut.Targets);
        }
    }
}
