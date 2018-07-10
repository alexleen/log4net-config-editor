// Copyright © 2018 Alex Leendertsen

using System.Linq;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Definitions.Appenders;
using Editor.Descriptors;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.Definitions.Appenders
{
    [TestFixture]
    public class AsyncAppenderTest
    {
        private AsyncAppender mSut;

        [SetUp]
        public void SetUp()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement log4NetNode = xmlDoc.CreateElement("log4net");

            IElementConfiguration configuration = Substitute.For<IElementConfiguration>();
            configuration.ConfigXml.Returns(xmlDoc);
            configuration.Log4NetNode.Returns(log4NetNode);

            mSut = new AsyncAppender(configuration);
        }

        [Test]
        public void Name_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("Async Appender", mSut.Name);
        }

        [Test]
        public void Icon_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("pack://application:,,,/Editor;component/Images/list-add.png", mSut.Icon);
        }

        [Test]
        public void Descriptor_ShouldReturnCorrectValue()
        {
            Assert.AreEqual(AppenderDescriptor.Async, mSut.Descriptor);
        }

        [Test]
        public void Initialize_ShouldAddDefaultProperties()
        {
            mSut.Initialize();

            TestHelpers.AssertDefaultPropertiesExist(mSut.Properties);
        }

        [Test]
        public void Initialize_ShouldAddForwardingProperties()
        {
            mSut.Initialize();

            mSut.Properties.Single(p => p.GetType() == typeof(OutgoingRefs));
        }

        [Test]
        public void Initialize_ShouldAddAsyncProperties()
        {
            mSut.Initialize();

            mSut.Properties.Single(p => p.GetType() == typeof(Fix));
            mSut.Properties.Single(p => p.GetType() == typeof(BufferSize));
        }

        [Test]
        public void FixProperty_ShouldDefaultToPartial()
        {
            mSut.Initialize();

            Fix fix = (Fix)mSut.Properties.Single(p => p.GetType() == typeof(Fix));

            Assert.AreEqual(Fix.PartialPreset, fix.SelectedPreset);
        }
    }
}
