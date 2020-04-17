// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class DatePatternTest
    {
        private DatePattern mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new DatePattern();
        }

        [TestCase(null)]
        [TestCase("")]
        public void TryValidate_ShouldShowMessageBoxAndReturnsFalse_WhenValueIsNullOrEmpty(string value)
        {
            mSut.Value = value;

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsFalse(mSut.TryValidate(messageBoxService));

            messageBoxService.Received(1).ShowError("A valid date pattern must be assigned.");
        }

        [TestCase("HH:mm", ":")]
        [TestCase("HH<mm", "<")]
        [TestCase("HH>mm", ">")]
        [TestCase("HH\"mm", "\"")]
        [TestCase("HH/mm", "/")]
        [TestCase("HH\\mm", "\\")]
        [TestCase("HH|mm", "|")]
        [TestCase("HH?mm", "?")]
        [TestCase("HH*mm", "*")]
        //There's a lot more here, but these are the most common
        //See https://stackoverflow.com/a/62888/7355697
        public void TryValidate_ShouldShowMessageBoxAndReturnsFalse_WhenValueHasInvalidPathChars(string value, string invalidChar)
        {
            mSut.Value = value;

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsFalse(mSut.TryValidate(messageBoxService));

            messageBoxService.Received(1).ShowError($"Date pattern must not contain invalid characters: '{invalidChar}'");
        }

        [Test]
        public void TryValidate_ShouldNotShowMessageBox_WhenValueIsNotNullOrEmpty()
        {
            mSut.Value = "yyyyMMdd";

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            mSut.TryValidate(messageBoxService);

            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }

        [Test]
        public void TryValidate_ShouldReturnTrue_WhenValueIsNotNullOrEmpty()
        {
            mSut.Value = "yyyyMMdd";

            Assert.IsTrue(mSut.TryValidate(Substitute.For<IMessageBoxService>()));
        }
    }
}
