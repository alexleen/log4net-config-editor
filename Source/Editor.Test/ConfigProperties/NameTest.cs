// Copyright © 2020 Alex Leendertsen

using System.Xml;
using Editor.ConfigProperties;
using Editor.Interfaces;
using Editor.Utilities;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class NameTest
    {
        private const string OriginalName = "appName";
        private Name mSut;
        private XmlNode mLog4NetNode;
        private XmlDocument mXmlDoc;
        private XmlNode mOriginalAppender;
        private IElementConfiguration mAppenderConfiguration;

        [SetUp]
        public void SetUp()
        {
            mXmlDoc = new XmlDocument();
            mLog4NetNode = mXmlDoc.CreateElement("log4net");
            mOriginalAppender = mXmlDoc.CreateElementWithAttribute("appender", "name", OriginalName);
            mOriginalAppender.AppendTo(mLog4NetNode);

            mAppenderConfiguration = Substitute.For<IElementConfiguration>();
            mAppenderConfiguration.ConfigXml.Returns(mXmlDoc);
            mAppenderConfiguration.Log4NetNode.Returns(mLog4NetNode);
            mAppenderConfiguration.OriginalNode.Returns(mOriginalAppender);

            mSut = new Name(mAppenderConfiguration);
        }

        [TestCase(null, false)]
        [TestCase("", true)]
        public void Load_ShouldNotLoadValue(string value, bool retVal)
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("name", out _).Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns(value);
                    ci[1] = result;
                    return retVal;
                });

            mSut.Load(config);

            Assert.IsNull(mSut.Value);
        }

        [Test]
        public void Changed_ShouldBeFalse_WhenValueDoesMatchOriginal()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("name", out _).Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns(OriginalName);
                    ci[1] = result;
                    return true;
                });

            mSut.Load(config);

            Assert.IsFalse(mSut.Changed);
        }

        [Test]
        public void Changed_ShouldBeNull_WhenNoOriginalNameExists()
        {
            mAppenderConfiguration.OriginalNode.Returns((XmlNode)null);

            Assert.IsNull(mSut.Changed);
        }

        [Test]
        public void Changed_ShouldBeTrue_WhenValueDoesNotMatchOriginal()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("name", out _).Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns(OriginalName);
                    ci[1] = result;
                    return true;
                });

            mSut.Load(config);

            mSut.Value = "someOtherName";

            Assert.IsTrue(mSut.Changed);
        }

        [Test]
        public void IsFocused_ShouldBeTrue()
        {
            Assert.IsTrue(mSut.IsFocused);
        }

        [Test]
        public void Load_ShouldLoadCorrectName()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("name", out _).Returns(ci =>
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
        public void OriginalName_ShouldBeNull_WhenNoOriginalNameAttrExists()
        {
            mAppenderConfiguration.OriginalNode.Attributes.RemoveNamedItem("name");

            Assert.IsNull(mSut.OriginalName);
        }

        [Test]
        public void OriginalName_ShouldBeNull_WhenNoOriginalNodeExists()
        {
            mAppenderConfiguration.OriginalNode.Returns((XmlNode)null);

            Assert.IsNull(mSut.OriginalName);
        }

        [Test]
        public void OriginalName_ShouldMatchOriginalName_WhenOriginalNameExists()
        {
            Assert.AreEqual(OriginalName, mSut.OriginalName);
        }

        [Test]
        public void Save_ShouldSaveNameToAttribute()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            const string nameValue = "name";
            mSut.Value = nameValue;

            mSut.Save(config);

            config.Received(1).Save("name", nameValue);
        }

        [TestCase("")]
        [TestCase(null)]
        public void TryValidate_ShouldShowUnassignedMessageBox_WhenValueIsNullOrEmpty_AndReturnFalse(string value)
        {
            mSut.Value = value;

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsFalse(mSut.TryValidate(messageBoxService));
            messageBoxService.Received(1).ShowError("A name must be assigned to this appender.");
        }

        [Test]
        public void TryValidate_ShouldNotShowCollisionMessageBox_WhenAppenderNameCollides_ButIsSameAppender()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("name", out _).Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns(OriginalName);
                    ci[1] = result;
                    return true;
                });

            mSut.Load(config);

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsTrue(mSut.TryValidate(messageBoxService));
            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }

        [Test]
        public void TryValidate_ShouldNotShowCollisionMessageBox_WhenNoAppenderNamesCollide()
        {
            mSut.Value = OriginalName;
            mXmlDoc.CreateElementWithAttribute("appender", "name", "otherName").AppendTo(mLog4NetNode);

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsTrue(mSut.TryValidate(messageBoxService));
            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }

        [Test]
        public void TryValidate_ShouldNotShowCollisionMessageBox_WhenOtherAppenderHasNoName()
        {
            mSut.Value = "appName";
            mXmlDoc.CreateElement("appender").AppendTo(mLog4NetNode);

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsTrue(mSut.TryValidate(messageBoxService));
            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }

        [Test]
        public void TryValidate_ShouldNotShowUnassignedMessageBox_WhenValueIsNotNullOrEmpty_AndReturnTrue()
        {
            mSut.Value = "name";

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsTrue(mSut.TryValidate(messageBoxService));
            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }

        [Test]
        public void TryValidate_ShouldShowCollisionMessageBox_WhenAppenderNameCollides()
        {
            mSut.Value = OriginalName;
            mXmlDoc.CreateElementWithAttribute("appender", "name", mSut.Value).AppendTo(mLog4NetNode);

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsFalse(mSut.TryValidate(messageBoxService));
            messageBoxService.Received(1).ShowError("Name must be unique.");
        }
    }
}
