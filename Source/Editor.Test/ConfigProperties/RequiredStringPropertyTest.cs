// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class RequiredStringPropertyTest
    {
        [SetUp]
        public void SetUp()
        {
            mSut = new RequiredStringProperty("name", "elementName");
        }

        private RequiredStringProperty mSut;

        [Test]
        public void TryValidate_ShouldReturnFalse_WhenValueNotSpecified()
        {
            Assert.IsFalse(mSut.TryValidate(Substitute.For<IMessageBoxService>()));
        }

        [Test]
        public void TryValidate_ShouldReturnTrue_WhenValueSpecified()
        {
            mSut.Value = "whatev";

            Assert.IsTrue(mSut.TryValidate(Substitute.For<IMessageBoxService>()));
        }

        [Test]
        public void TryValidate_ShouldShowError_WhenValueNotSpecified()
        {
            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            mSut.TryValidate(messageBoxService);

            messageBoxService.Received(1).ShowError(Arg.Is<string>(arg => arg == "'name' must be specified."));
        }
    }
}
