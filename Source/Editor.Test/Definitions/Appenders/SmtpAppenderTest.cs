// Copyright © 2020 Alex Leendertsen

using System.Linq;
using System.Net.Mail;
using System.Xml;
using Editor.ConfigProperties;
using Editor.ConfigProperties.Base;
using Editor.Definitions.Appenders;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Utilities;
using NSubstitute;
using NUnit.Framework;
using static log4net.Appender.SmtpAppender;

namespace Editor.Test.Definitions.Appenders
{
    [TestFixture]
    public class SmtpAppenderTest
    {
        private SmtpAppender mSut;

        [SetUp]
        public void SetUp()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement log4NetNode = xmlDoc.CreateElement(Log4NetXmlConstants.Log4Net);

            IElementConfiguration configuration = Substitute.For<IElementConfiguration>();
            configuration.ConfigXml.Returns(xmlDoc);
            configuration.Log4NetNode.Returns(log4NetNode);

            mSut = new SmtpAppender(configuration);
            mSut.Initialize();
        }

        [Test]
        public void Name_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("SMTP Appender", mSut.Name);
        }

        [Test]
        public void Icon_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("pack://application:,,,/Editor;component/Images/list-add.png", mSut.Icon);
        }

        [Test]
        public void Descriptor_ShouldReturnCorrectValue()
        {
            Assert.AreEqual(AppenderDescriptor.Smtp, mSut.Descriptor);
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
        public void Initialize_ShouldAddSmtpProperties()
        {
            //Single throws if the specified type is not found, which is good enough to fail the test
            //No asserts needed
            mSut.Properties.Single(p => p.GetType() == typeof(RequiredStringProperty) && ((RequiredStringProperty)p).Name == "Host:");
            mSut.Properties.Single(p => p.GetType() == typeof(Port));
            mSut.Properties.Single(p => p.GetType() == typeof(EnumProperty<SmtpAuthentication>) && ((EnumProperty<SmtpAuthentication>)p).Name == "Authentication:");
            mSut.Properties.Single(p => p.GetType() == typeof(BooleanPropertyBase) && ((BooleanPropertyBase)p).Name == "Enable SSL:");
            mSut.Properties.Single(p => p.GetType() == typeof(RequiredStringProperty) && ((RequiredStringProperty)p).Name == "To:");
            mSut.Properties.Single(p => p.GetType() == typeof(RequiredStringProperty) && ((RequiredStringProperty)p).Name == "From:");
            mSut.Properties.Single(p => p.GetType() == typeof(StringValueProperty) && ((StringValueProperty)p).Name == "Reply To:");
            mSut.Properties.Single(p => p.GetType() == typeof(StringValueProperty) && ((StringValueProperty)p).Name == "Cc:");
            mSut.Properties.Single(p => p.GetType() == typeof(StringValueProperty) && ((StringValueProperty)p).Name == "Bcc:");
            mSut.Properties.Single(p => p.GetType() == typeof(StringValueProperty) && ((StringValueProperty)p).Name == "Subject:");
            mSut.Properties.Single(p => p.GetType() == typeof(Encoding) && ((Encoding)p).Name == "Subject Encoding:");
            mSut.Properties.Single(p => p.GetType() == typeof(Encoding) && ((Encoding)p).Name == "Body Encoding:");
            mSut.Properties.Single(p => p.GetType() == typeof(EnumProperty<MailPriority>) && ((EnumProperty<MailPriority>)p).Name == "Priority:");
        }

        [Test]
        public void Initialize_ShouldAddCorrectNumberOfProperties()
        {
            Assert.AreEqual(25, mSut.Properties.Count);
        }

        [Test]
        public void Initialize_ShouldNotIncludeUsernameAndPassword()
        {
            //Should NOT include username and password to start (since auth defaults to basic)
            CollectionAssert.IsEmpty(mSut.Properties.Where(p => p.GetType() == typeof(RequiredStringProperty) && ((RequiredStringProperty)p).Name == "Username:"));
            CollectionAssert.IsEmpty(mSut.Properties.Where(p => p.GetType() == typeof(Password)));
        }

        [Test]
        public void Sut_ShouldAddUsernameAndPassword_WhenBasic()
        {
            ChangeToBasic();

            //Check to make sure everything else is still there
            Initialize_ShouldAddAppenderSkeletonProperties();
            Initialize_ShouldAddBufferingAppenderSkeletonProperties();
            Initialize_ShouldAddSmtpProperties();

            mSut.Properties.Single(p => p.GetType() == typeof(RequiredStringProperty) && ((RequiredStringProperty)p).Name == "Username:");
            mSut.Properties.Single(p => p.GetType() == typeof(Password));
        }

        [Test]
        public void Sut_ShouldAddUsernameAndPassword_AtUniqueIndex_WhenBasic()
        {
            ChangeToBasic();

            Assert.AreEqual(mSut.Properties.Count, mSut.Properties.Select(p => p.RowIndex).Distinct().Count());
        }

        private void ChangeToBasic()
        {
            ((EnumProperty<SmtpAuthentication>)mSut.Properties.First(p => p.GetType() == typeof(EnumProperty<SmtpAuthentication>))).SelectedValue = SmtpAuthentication.Basic.ToString();
        }
    }
}
