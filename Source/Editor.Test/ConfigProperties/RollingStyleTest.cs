// Copyright © 2020 Alex Leendertsen

using System;
using System.Linq;
using Editor.ConfigProperties;
using Editor.Interfaces;
using log4net.Appender;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class RollingStyleTest
    {
        [SetUp]
        public void SetUp()
        {
            mSut = new RollingStyle();
        }

        private RollingStyle mSut;

        [TestCase(null)]
        [TestCase("")]
        [TestCase("whatev")]
        public void Load_ShouldNotLoadMode(string mode)
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("value", out _, "rollingStyle").Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns(mode);
                    ci[1] = result;
                    return true;
                });

            mSut.Load(config);

            Assert.AreEqual(RollingFileAppender.RollingMode.Composite, mSut.SelectedMode);
        }

        [Test]
        public void Load_ShouldLoadMode()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("value", out _, "rollingStyle").Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns("Date");
                    ci[1] = result;
                    return true;
                });

            mSut.Load(config);

            Assert.AreEqual(RollingFileAppender.RollingMode.Date, mSut.SelectedMode);
        }

        [Test]
        public void Modes_ShouldBeInitializedCorrectly()
        {
            CollectionAssert.AreEqual(Enum.GetValues(typeof(RollingFileAppender.RollingMode)).Cast<RollingFileAppender.RollingMode>(), mSut.Modes);
        }

        [Test]
        public void Save_ShouldNotSave_WhenComposite()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.Save(config);

            config.DidNotReceive().Save();
        }

        [Test]
        public void Save_ShouldSave_WhenNotComposite()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.SelectedMode = RollingFileAppender.RollingMode.Date;
            mSut.Save(config);

            config.Received(1).Save(("rollingStyle", "value", "Date"));
        }

        [Test]
        public void SelectedMode_ShouldBeInitializedToComposite()
        {
            Assert.AreEqual(RollingFileAppender.RollingMode.Composite, mSut.SelectedMode);
        }

        [Test]
        public void SelectedMode_ShouldFirePropChange_AndChange_WhenValueHasChanged()
        {
            bool fired = false;
            mSut.PropertyChanged += (sender, args) => { fired = true; };

            mSut.SelectedMode = RollingFileAppender.RollingMode.Date;

            Assert.IsTrue(fired);
            Assert.AreEqual(RollingFileAppender.RollingMode.Date, mSut.SelectedMode);
        }

        [Test]
        public void SelectedMode_ShouldNotFirePropChange_WhenValueHasNotChanged()
        {
            bool fired = false;
            mSut.PropertyChanged += (sender, args) => { fired = true; };

            mSut.SelectedMode = mSut.SelectedMode;

            Assert.IsFalse(fired);
        }
    }
}
