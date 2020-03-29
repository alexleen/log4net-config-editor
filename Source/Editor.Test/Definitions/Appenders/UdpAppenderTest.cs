// Copyright © 2020 Alex Leendertsen

using System.Linq;
using System.Xml;
using Editor.ConfigProperties;
using Editor.ConfigProperties.Base;
using Editor.Definitions.Appenders;
using Editor.Descriptors;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.Definitions.Appenders
{
    [TestFixture]
    public class UdpAppenderTest
    {
        private UdpAppender mSut;

        [SetUp]
        public void SetUp()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement log4NetNode = xmlDoc.CreateElement("log4net");

            IElementConfiguration configuration = Substitute.For<IElementConfiguration>();
            configuration.ConfigXml.Returns(xmlDoc);
            configuration.Log4NetNode.Returns(log4NetNode);

            mSut = new UdpAppender(configuration);
        }

        [Test]
        public void Name_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("UDP Appender", mSut.Name);
        }

        [Test]
        public void Icon_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("pack://application:,,,/Editor;component/Images/list-add.png", mSut.Icon);
        }

        [Test]
        public void Descriptor_ShouldReturnCorrectValue()
        {
            Assert.AreEqual(AppenderDescriptor.Udp, mSut.Descriptor);
        }

        [Test]
        public void Initialize_ShouldAddDefaultProperties()
        {
            mSut.Initialize();

            TestHelpers.AssertAppenderSkeletonPropertiesExist(mSut.Properties);
        }

        [Test]
        public void Initialize_ShouldAddUdpProperties()
        {
            mSut.Initialize();

            mSut.Properties.Single(p => p.GetType() == typeof(RemoteAddress));
            Assert.AreEqual(2, mSut.Properties.Count(p => p.GetType() == typeof(NumericProperty<ushort>)));
        }

        [Test]
        public void Initialize_ShouldAddCorrectNumberOfProperties()
        {
            mSut.Initialize();

            Assert.AreEqual(TestHelpers.AppenderSkeletonPropertyCount + 3, mSut.Properties.Count);
        }
    }
}
