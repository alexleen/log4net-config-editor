// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class RenderingClassTest
    {
        [SetUp]
        public void SetUp()
        {
            mSut = new RenderingClass();
        }

        private RenderingClass mSut;

        [Test]
        public void Load_ShouldNotLoadRenderingClass()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("renderingClass", out _).Returns(false);

            mSut.Load(config);

            Assert.IsNull(mSut.Value);
        }

        [TestCase("")]
        [TestCase(null)]
        public void TryValidate_ShouldShowUnassignedMessageBox_WhenValueIsNullOrEmpty_AndReturnFalse(string value)
        {
            mSut.Value = value;

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsFalse(mSut.TryValidate(messageBoxService));
            messageBoxService.Received(1).ShowError("A valid rendering class must be assigned.");
        }

        [Test]
        public void IsFocused_ShouldBeTrue()
        {
            Assert.IsTrue(mSut.IsFocused);
        }

        [Test]
        public void Load_ShouldLoadCorrectRenderingClass()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("renderingClass", out _).Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns("ColoredConsoleAppender");
                    ci[1] = result;
                    return true;
                });

            mSut.Load(config);

            Assert.AreEqual("ColoredConsoleAppender", mSut.Value);
        }

        [Test]
        public void Name_ShouldBeInitializedCorrectly()
        {
            Assert.AreEqual("Rendering Class:", mSut.Name);
        }

        [Test]
        public void Save_ShouldSaveRenderingClassToAttribute()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            const string renderingClass = "class";
            mSut.Value = renderingClass;

            mSut.Save(config);

            config.Received(1).Save("renderingClass", renderingClass);
        }

        [Test]
        public void TryValidate_ShouldNotShowUnassignedMessageBox_WhenValueIsNotNullOrEmpty_AndReturnTrue()
        {
            mSut.Value = "class";

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsTrue(mSut.TryValidate(messageBoxService));
            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }
    }
}
