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

namespace Editor.Test.Definitions.Appenders
{
    [TestFixture]
    public class EventLogAppenderTest
    {
        private EventLogAppender mSut;

        [SetUp]
        public void SetUp()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement log4NetNode = xmlDoc.CreateElement(Log4NetXmlConstants.Log4Net);

            IElementConfiguration configuration = Substitute.For<IElementConfiguration>();
            configuration.ConfigXml.Returns(xmlDoc);
            configuration.Log4NetNode.Returns(log4NetNode);

            mSut = new EventLogAppender(configuration);
        }

        [Test]
        public void Name_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("Event Log Appender", mSut.Name);
        }

        [Test]
        public void Icon_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("pack://application:,,,/Editor;component/Images/list-add.png", mSut.Icon);
        }

        [Test]
        public void Descriptor_ShouldReturnCorrectValue()
        {
            Assert.AreEqual(AppenderDescriptor.EventLog, mSut.Descriptor);
        }

        [Test]
        public void Initialize_ShouldAddDefaultProperties()
        {
            mSut.Initialize();

            TestHelpers.AssertAppenderSkeletonPropertiesExist(mSut.Properties);
        }

        [Test]
        public void Initialize_ShouldAddEventLogProperties()
        {
            mSut.Initialize();

            Assert.AreEqual(2, mSut.Properties.Count(p => p.GetType() == typeof(RequiredStringProperty)));
            mSut.Properties.Single(p => p.GetType() == typeof(NumericProperty<short>) && ((NumericProperty<short>)p).Name == "Category:");
            mSut.Properties.Single(p => p.GetType() == typeof(NumericProperty<int>) && ((NumericProperty<int>)p).Name == "Event Id:");
            mSut.Properties.Single(p => p.GetType() == typeof(StringValueProperty) && ((StringValueProperty)p).Name == "Security Context:");
        }

        [Test]
        public void Initialize_ShouldAddCorrectNumberOfProperties()
        {
            mSut.Initialize();

            Assert.AreEqual(TestHelpers.AppenderSkeletonPropertyCount + 5, mSut.Properties.Count);
        }
    }
}
