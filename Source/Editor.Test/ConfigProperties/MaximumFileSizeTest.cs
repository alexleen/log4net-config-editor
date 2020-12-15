// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Interfaces;
using Editor.XML;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class MaximumFileSizeTest
    {
        private MaximumFileSize mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new MaximumFileSize();
        }

        [TestCase("10KB")]
        [TestCase("10MB")]
        [TestCase("10GB")]
        public void TryValidate_ShouldSucceed(string value)
        {
            mSut.Value = value;

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();
            Assert.IsTrue(mSut.TryValidate(messageBoxService));
            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }

        [TestCase("10kb")]
        [TestCase("10")]
        [TestCase("10Gb")]
        public void TryValidate_ShouldFail(string value)
        {
            mSut.Value = value;

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();
            Assert.IsFalse(mSut.TryValidate(messageBoxService));
            messageBoxService.Received(1).ShowError("Maximum file size must end with either \"KB\", \"MB\", or \"GB\".");
        }

        [Test]
        public void Save_ShouldNotSaveIfDefault()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.Save(config);

            config.DidNotReceiveWithAnyArgs().Save();
            config.DidNotReceiveWithAnyArgs().Save();
        }

        [Test]
        public void Save_ShouldSaveIfNotDefault()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.Value = "100MB";
            mSut.Save(config);

            config.Received(1).Save(new Element("maximumFileSize", new[] { ("value", mSut.Value) }));
        }

        [Test]
        public void ToolTip_ShouldBeInitialized()
        {
            Assert.IsNotNull(mSut.ToolTip);
        }

        [Test]
        public void Value_ShouldBeInitializedToDefault()
        {
            Assert.AreEqual("10MB", mSut.Value);
        }
    }
}
