// Copyright © 2020 Alex Leendertsen

using System.Xml;
using Editor.ConfigProperties;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class RenderedClassTest
    {
        [SetUp]
        public void SetUp()
        {
            mSut = new RenderedClass();
        }

        private RenderedClass mSut;

        [TestCase(null)]
        [TestCase("")]
        public void Load_ShouldNotLoadRenderedClass(string renderedClass)
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("renderedClass", out _).Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns(renderedClass);
                    ci[1] = result;
                    return false;
                });

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
            messageBoxService.Received(1).ShowError("A valid rendered class must be assigned.");
        }

        [Test]
        public void Load_ShouldLoadCorrectRenderedClass()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("renderedClass", out _).Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns("ColoredConsoleAppender");
                    ci[1] = result;
                    return false;
                });

            mSut.Load(config);

            Assert.AreEqual("ColoredConsoleAppender", mSut.Value);
        }

        [Test]
        public void Name_ShouldBeInitializedCorrectly()
        {
            Assert.AreEqual("Rendered Class:", mSut.Name);
        }

        [Test]
        public void Save_ShouldSaveRenderedClassToAttribute()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("renderer");

            const string renderedClass = "class";
            mSut.Value = renderedClass;

            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            mSut.Save(config);

            config.Received(1).Save("renderedClass", renderedClass);
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
