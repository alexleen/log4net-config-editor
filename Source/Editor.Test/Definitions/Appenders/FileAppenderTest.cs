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
    public class FileAppenderTest
    {
        private FileAppender mSut;

        [SetUp]
        public void SetUp()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement log4NetNode = xmlDoc.CreateElement(Log4NetXmlConstants.Log4Net);

            IElementConfiguration configuration = Substitute.For<IElementConfiguration>();
            configuration.ConfigXml.Returns(xmlDoc);
            configuration.Log4NetNode.Returns(log4NetNode);

            mSut = new FileAppender(configuration);
        }

        [Test]
        public void Name_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("File Appender", mSut.Name);
        }

        [Test]
        public void Icon_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("pack://application:,,,/Editor;component/Images/list-add.png", mSut.Icon);
        }

        [Test]
        public void Descriptor_ShouldReturnCorrectValue()
        {
            Assert.AreEqual(AppenderDescriptor.File, mSut.Descriptor);
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
        public void Initialize_ShouldAddCorrectNumberOfProperties()
        {
            mSut.Initialize();

            Assert.AreEqual(TestHelpers.AppenderSkeletonPropertyCount + 7, mSut.Properties.Count);
        }
    }
}
