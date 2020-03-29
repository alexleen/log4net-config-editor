// Copyright © 2020 Alex Leendertsen

using System.Linq;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Definitions.Appenders;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Utilities;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.Definitions.Appenders
{
    [TestFixture]
    public class SmtpPickupDirAppenderTest
    {
        private SmtpPickupDirAppender mSut;

        [SetUp]
        public void SetUp()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement log4NetNode = xmlDoc.CreateElement(Log4NetXmlConstants.Log4Net);

            IElementConfiguration configuration = Substitute.For<IElementConfiguration>();
            configuration.ConfigXml.Returns(xmlDoc);
            configuration.Log4NetNode.Returns(log4NetNode);

            mSut = new SmtpPickupDirAppender(configuration);
            mSut.Initialize();
        }

        [Test]
        public void Name_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("SMTP Pickup Dir Appender", mSut.Name);
        }

        [Test]
        public void Icon_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("pack://application:,,,/Editor;component/Images/list-add.png", mSut.Icon);
        }

        [Test]
        public void Descriptor_ShouldReturnCorrectValue()
        {
            Assert.AreEqual(AppenderDescriptor.SmtpPickupDir, mSut.Descriptor);
        }

        [Test]
        public void Initialize_ShouldAddAppenderSkeletonProperties()
        {
            TestHelpers.AssertAppenderSkeletonPropertiesExist(mSut.Properties);
        }

        [Test]
        public void Initialize_ShouldAddBufferingAppenderSkeletonProperties()
        {
            TestHelpers.AssertBufferingAppenderSkeletonPropertiesExist(mSut.Properties);
        }

        [Test]
        public void Initialize_ShouldAddProperties()
        {
            mSut.Properties.Single(p => p.GetType() == typeof(RequiredStringProperty) && ((RequiredStringProperty)p).Name == "File Extension:");
            mSut.Properties.Single(p => p.GetType() == typeof(RequiredStringProperty) && ((RequiredStringProperty)p).Name == "Pickup Dir:");
            mSut.Properties.Single(p => p.GetType() == typeof(RequiredStringProperty) && ((RequiredStringProperty)p).Name == "To:");
            mSut.Properties.Single(p => p.GetType() == typeof(RequiredStringProperty) && ((RequiredStringProperty)p).Name == "From:");
            mSut.Properties.Single(p => p.GetType() == typeof(RequiredStringProperty) && ((RequiredStringProperty)p).Name == "Subject:");
        }

        [Test]
        public void Initialize_ShouldAddCorrectNumberOfProperties()
        {
            Assert.AreEqual(17, mSut.Properties.Count);
        }
    }
}
