// Copyright © 2020 Alex Leendertsen

using System.Linq;
using System.Xml;
using Editor.ConfigProperties;
using Editor.ConfigProperties.Base;
using Editor.Definitions.Appenders;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Utilities;
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
            XmlElement log4NetNode = xmlDoc.CreateElement(Log4NetXmlConstants.Log4Net);

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

            TestHelpers.AssertAppenderSkeletonPropertiesExist(mSut.Properties);
        }

        [Test]
        public void Initialize_ShouldAddTextWriterProperties()
        {
            mSut.Initialize();

            mSut.Properties.Single(p => p.GetType() == typeof(BooleanPropertyBase) && ((BooleanPropertyBase)p).Name == "Immediate Flush:");
            mSut.Properties.Single(p => p.GetType() == typeof(StringValueProperty) && ((StringValueProperty)p).Name == "Quiet Writer:");
            mSut.Properties.Single(p => p.GetType() == typeof(StringValueProperty) && ((StringValueProperty)p).Name == "Writer:");
        }

        [Test]
        public void Initialize_ShouldAddFileProperties()
        {
            mSut.Initialize();

            mSut.Properties.Single(p => p.GetType() == typeof(File));
            mSut.Properties.Single(p => p.GetType() == typeof(LockingModel));
            mSut.Properties.Single(p => p.GetType() == typeof(Encoding));
            mSut.Properties.Single(p => p.GetType() == typeof(StringValueProperty) && ((StringValueProperty)p).Name == "Security Context:");
        }

        [Test]
        public void Initialize_ShouldAddRollingFileProperties()
        {
            mSut.Initialize();

            mSut.Properties.Single(p => p.GetType() == typeof(RollingStyle));
            mSut.Properties.Single(p => p.GetType() == typeof(DatePattern));
            Assert.AreEqual(3, mSut.Properties.Count(p => p.GetType() == typeof(BooleanPropertyBase)));
            mSut.Properties.Single(p => p.GetType() == typeof(MaximumFileSize));
            mSut.Properties.Single(p => p.GetType() == typeof(MaxSizeRollBackups));
            mSut.Properties.Single(p => p.GetType() == typeof(CountDirection));
        }

        [Test]
        public void Initialize_ShouldAddCorrectNumberOfProperties()
        {
            mSut.Initialize();

            Assert.AreEqual(21, mSut.Properties.Count);
        }

        [TestCase(RollingMode.Once, false)]
        [TestCase(RollingMode.Size, false)]
        [TestCase(RollingMode.Date, true)]
        [TestCase(RollingMode.Composite, true)]
        public void DatePattern_ShouldBeAddedRemoved_BasedOnRollingMode(RollingMode mode, bool present)
        {
            mSut.Initialize();

            RollingStyle rollingStyle = (RollingStyle)mSut.Properties.Single(p => p.GetType() == typeof(RollingStyle));
            rollingStyle.SelectedMode = mode;

            Assert.AreEqual(present, mSut.Properties.FirstOrDefault(p => p.GetType() == typeof(DatePattern)) != null);
        }

        [TestCase(RollingMode.Once, false)]
        [TestCase(RollingMode.Size, true)]
        [TestCase(RollingMode.Date, false)]
        [TestCase(RollingMode.Composite, true)]
        public void MaximumFileSize_ShouldBeAddedRemoved_BasedOnRollingMode(RollingMode mode, bool present)
        {
            mSut.Initialize();

            RollingStyle rollingStyle = (RollingStyle)mSut.Properties.Single(p => p.GetType() == typeof(RollingStyle));
            rollingStyle.SelectedMode = mode;

            Assert.AreEqual(present, mSut.Properties.FirstOrDefault(p => p.GetType() == typeof(MaximumFileSize)) != null);
        }

        [TestCase(RollingMode.Once, true)]
        [TestCase(RollingMode.Size, true)]
        [TestCase(RollingMode.Date, false)]
        [TestCase(RollingMode.Composite, true)]
        public void CountDirection_ShouldBeAddedRemoved_BasedOnRollingMode(RollingMode mode, bool present)
        {
            mSut.Initialize();

            RollingStyle rollingStyle = (RollingStyle)mSut.Properties.Single(p => p.GetType() == typeof(RollingStyle));
            rollingStyle.SelectedMode = mode;

            Assert.AreEqual(present, mSut.Properties.FirstOrDefault(p => p.GetType() == typeof(CountDirection)) != null);
        }

        [Test]
        public void Properties_ShouldBeAddedToTheCorrectIndex_WhenAddedBasedOnRollingMode()
        {
            mSut.Initialize();

            int dateIndex = mSut.Properties.IndexOf(mSut.Properties.Single(p => p.GetType() == typeof(DatePattern)));
            int staticIndex = mSut.Properties.IndexOf(mSut.Properties.Single(p => p is BooleanPropertyBase bpb && bpb.Name == "Static Log File Name:"));

            RollingStyle rollingStyle = (RollingStyle)mSut.Properties.Single(p => p.GetType() == typeof(RollingStyle));

            //Remove two properties
            rollingStyle.SelectedMode = RollingMode.Once;

            //Add them back
            rollingStyle.SelectedMode = RollingMode.Composite;

            Assert.AreEqual(dateIndex, mSut.Properties.IndexOf(mSut.Properties.Single(p => p.GetType() == typeof(DatePattern))));
            Assert.AreEqual(staticIndex, mSut.Properties.IndexOf(mSut.Properties.Single(p => p is BooleanPropertyBase bpb && bpb.Name == "Static Log File Name:")));
        }

        [TestCase(RollingMode.Once, 19)]
        [TestCase(RollingMode.Size, 20)]
        [TestCase(RollingMode.Date, 19)]
        [TestCase(RollingMode.Composite, 21)]
        public void Properties_ShouldNotBeDuplicated_WhenTheyAlreadyExist(RollingMode mode, int expectedCount)
        {
            mSut.Initialize();

            RollingStyle rollingStyle = (RollingStyle)mSut.Properties.Single(p => p.GetType() == typeof(RollingStyle));

            rollingStyle.SelectedMode = mode;

            Assert.AreEqual(expectedCount, mSut.Properties.Count);
        }
    }
}
