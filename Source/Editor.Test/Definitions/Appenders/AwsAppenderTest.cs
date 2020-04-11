// Copyright © 2020 Alex Leendertsen

using System.Linq;
using System.Xml;
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
    public class AwsAppenderTest
    {
        private AwsAppender mSut;

        [SetUp]
        public void SetUp()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement log4NetNode = xmlDoc.CreateElement(Log4NetXmlConstants.Log4Net);

            IElementConfiguration configuration = Substitute.For<IElementConfiguration>();
            configuration.ConfigXml.Returns(xmlDoc);
            configuration.Log4NetNode.Returns(log4NetNode);

            mSut = new AwsAppender(configuration);
            mSut.Initialize();
        }

        [Test]
        public void Name_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("AWS Appender", mSut.Name);
        }

        [Test]
        public void Icon_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("pack://application:,,,/Editor;component/Images/list-add.png", mSut.Icon);
        }

        [Test]
        public void Descriptor_ShouldReturnCorrectValue()
        {
            Assert.AreEqual(AppenderDescriptor.AwsAppender, mSut.Descriptor);
        }

        [Test]
        public void Initialize_ShouldAddAppenderSkeletonProperties()
        {
            TestHelpers.AssertAppenderSkeletonPropertiesExist(mSut.Properties);
        }

        [Test]
        public void Initialize_ShouldAddCategoryProperty()
        {
            mSut.Properties.Single(p => p is StringValueProperty svp && svp.Name == "Log Group:");
            mSut.Properties.Single(p => p is BooleanPropertyBase bpb && bpb.Name == "Disable Log Group Creation:");
            mSut.Properties.Single(p => p is StringValueProperty svp && svp.Name == "Profile:");
            mSut.Properties.Single(p => p is StringValueProperty svp && svp.Name == "Profiles Location:");
            mSut.Properties.Single(p => p is StringValueProperty svp && svp.Name == "Credentials:");
            mSut.Properties.Single(p => p is StringValueProperty svp && svp.Name == "Region:");
            mSut.Properties.Single(p => p is StringValueProperty svp && svp.Name == "Service URL:");
            mSut.Properties.Single(p => p is StringValueProperty svp && svp.Name == "Batch Push Interval:");
            mSut.Properties.Single(p => p is NumericProperty<int> npb && npb.Name == "Batch Size In Bytes:");
            mSut.Properties.Single(p => p is NumericProperty<int> npb && npb.Name == "Max Queued Messages:");
            mSut.Properties.Single(p => p is StringValueProperty svp && svp.Name == "Log Stream Name Suffix:");
            mSut.Properties.Single(p => p is StringValueProperty svp && svp.Name == "Log Stream Name Prefix:");
            mSut.Properties.Single(p => p is StringValueProperty svp && svp.Name == "Library Log File Name:");
        }

        [Test]
        public void Initialize_ShouldAddCorrectNumberOfProperties()
        {
            Assert.AreEqual(TestHelpers.AppenderSkeletonPropertyCount + 13, mSut.Properties.Count);
        }

        [Test]
        public void Initialize_ShouldSetDefaults()
        {
            mSut.Properties.Single(p => p is NumericProperty<int> npb && npb.Name == "Batch Size In Bytes:" && npb.Value == "102400");
            mSut.Properties.Single(p => p is NumericProperty<int> npb && npb.Name == "Max Queued Messages:" && npb.Value == "10000");
            mSut.Properties.Single(p => p is StringValueProperty svp && svp.Name == "Library Log File Name:" && svp.Value == "aws-logger-errors.txt");
        }
    }
}
