// Copyright © 2018 Alex Leendertsen

using System.Linq;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Definitions.Appenders;
using Editor.Descriptors;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;
using static log4net.Appender.LocalSyslogAppender;

namespace Editor.Test.Definitions.Appenders
{
    [TestFixture]
    public class LocalSyslogAppenderTest
    {
        private LocalSyslogAppender mSut;

        [SetUp]
        public void SetUp()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement log4NetNode = xmlDoc.CreateElement("log4net");

            IElementConfiguration configuration = Substitute.For<IElementConfiguration>();
            configuration.ConfigXml.Returns(xmlDoc);
            configuration.Log4NetNode.Returns(log4NetNode);

            mSut = new LocalSyslogAppender(configuration);
        }

        [Test]
        public void Name_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("Local Syslog Appender", mSut.Name);
        }

        [Test]
        public void Icon_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("pack://application:,,,/Editor;component/Images/list-add.png", mSut.Icon);
        }

        [Test]
        public void Descriptor_ShouldReturnCorrectValue()
        {
            Assert.AreEqual(AppenderDescriptor.LocalSyslog, mSut.Descriptor);
        }

        [Test]
        public void Initialize_ShouldAddDefaultProperties()
        {
            mSut.Initialize();

            TestHelpers.AssertAppenderSkeletonPropertiesExist(mSut.Properties);
        }

        [Test]
        public void Initialize_ShouldAddLocalSyslogProperties()
        {
            mSut.Initialize();

            mSut.Properties.Single(p => p.GetType() == typeof(EnumProperty<SyslogFacility>));
            mSut.Properties.Single(p => p.GetType() == typeof(StringValueProperty));
        }

        [Test]
        public void Initialize_ShouldAddCorrectNumberOfProperties()
        {
            mSut.Initialize();

            Assert.AreEqual(9, mSut.Properties.Count);
        }
    }
}
