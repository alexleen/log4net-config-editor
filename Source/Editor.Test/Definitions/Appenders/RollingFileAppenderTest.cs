// Copyright © 2018 Alex Leendertsen

using System.Linq;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Definitions.Appenders;
using Editor.Descriptors;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;
using static log4net.Appender.RollingFileAppender;

namespace Editor.Test.Definitions.Appenders
{
    [TestFixture]
    public class RollingFileAppenderTest
    {
        private RollingFileAppender mSut;

        [SetUp]
        public void SetUp()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement log4NetNode = xmlDoc.CreateElement("log4net");

            IElementConfiguration configuration = Substitute.For<IElementConfiguration>();
            configuration.ConfigXml.Returns(xmlDoc);
            configuration.Log4NetNode.Returns(log4NetNode);

            mSut = new RollingFileAppender(configuration);
        }

        [Test]
        public void Name_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("Rolling File Appender", mSut.Name);
        }

        [Test]
        public void Icon_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("pack://application:,,,/Editor;component/Images/list-add.png", mSut.Icon);
        }

        [Test]
        public void Descriptor_ShouldReturnCorrectValue()
        {
            Assert.AreEqual(AppenderDescriptor.RollingFile, mSut.Descriptor);
        }

        [Test]
        public void Initialize_ShouldAddDefaultProperties()
        {
            mSut.Initialize();

            TestHelpers.AssertDefaultPropertiesExist(mSut.Properties);
        }

        [Test]
        public void Initialize_ShouldAddFileProperties()
        {
            mSut.Initialize();

            mSut.Properties.Single(p => p.GetType() == typeof(File));
            mSut.Properties.Single(p => p.GetType() == typeof(LockingModel));
        }

        [Test]
        public void Initialize_ShouldAddRollingFileProperties()
        {
            mSut.Initialize();

            mSut.Properties.Single(p => p.GetType() == typeof(RollingStyle));
            mSut.Properties.Single(p => p.GetType() == typeof(DatePattern));
            mSut.Properties.Single(p => p.GetType() == typeof(StaticLogFileName));
            mSut.Properties.Single(p => p.GetType() == typeof(PreserveExtension));
            mSut.Properties.Single(p => p.GetType() == typeof(MaximumFileSize));
            mSut.Properties.Single(p => p.GetType() == typeof(MaxSizeRollBackups));
            mSut.Properties.Single(p => p.GetType() == typeof(CountDirection));
        }

        [Test]
        public void Initialize_ShouldAddCorrectNumberOfProperties()
        {
            mSut.Initialize();

            Assert.AreEqual(16, mSut.Properties.Count);
        }

        [TestCase(RollingMode.Once, false)]
        [TestCase(RollingMode.Size, false)]
        [TestCase(RollingMode.Date, true)]
        [TestCase(RollingMode.Composite, true)]
        public void Properties_ShouldBeAddedRemoved_BasedOnRollingMode(RollingMode mode, bool present)
        {
            mSut.Initialize();

            RollingStyle rollingStyle = (RollingStyle)mSut.Properties.Single(p => p.GetType() == typeof(RollingStyle));
            rollingStyle.SelectedMode = mode;

            Assert.AreEqual(present, mSut.Properties.FirstOrDefault(p => p.GetType() == typeof(DatePattern)) != null);
            Assert.AreEqual(present, mSut.Properties.FirstOrDefault(p => p.GetType() == typeof(StaticLogFileName)) != null);
        }

        [Test]
        public void Properties_ShouldBeAddedToTheCorrectIndex_WhenAddedBasedOnRollingMode()
        {
            mSut.Initialize();

            int dateIndex = mSut.Properties.IndexOf(mSut.Properties.Single(p => p.GetType() == typeof(DatePattern)));
            int staticIndex = mSut.Properties.IndexOf(mSut.Properties.Single(p => p.GetType() == typeof(StaticLogFileName)));

            RollingStyle rollingStyle = (RollingStyle)mSut.Properties.Single(p => p.GetType() == typeof(RollingStyle));

            //Remove two properties
            rollingStyle.SelectedMode = RollingMode.Once;

            //Add them back
            rollingStyle.SelectedMode = RollingMode.Composite;

            Assert.AreEqual(dateIndex, mSut.Properties.IndexOf(mSut.Properties.Single(p => p.GetType() == typeof(DatePattern))));
            Assert.AreEqual(staticIndex, mSut.Properties.IndexOf(mSut.Properties.Single(p => p.GetType() == typeof(StaticLogFileName))));
        }

        [Test]
        public void Properties_ShouldNotBeDuplicated_WhenTheyAlreadyExist()
        {
            mSut.Initialize();

            RollingStyle rollingStyle = (RollingStyle)mSut.Properties.Single(p => p.GetType() == typeof(RollingStyle));

            //Composite is the default, and Date also specifies these two properties.
            rollingStyle.SelectedMode = RollingMode.Date;

            Assert.AreEqual(16, mSut.Properties.Count);
        }
    }
}
