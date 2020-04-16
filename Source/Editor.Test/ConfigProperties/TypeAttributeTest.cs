// Copyright © 2020 Alex Leendertsen

using System.Xml;
using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class TypeAttributeTest
    {
        [SetUp]
        public void SetUp()
        {
            mSut = new TypeAttribute(AppenderDescriptor.Async);
        }

        private TypeAttribute mSut;

        [TestCase(null)]
        [TestCase("")]
        [TestCase("type=\"\"")]
        public void Load_ShouldNotLoadType_RegularCtor(string xml)
        {
            mSut = new TypeAttribute();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml($"<appender name=\"ColoredConsoleAppender\" {xml}>\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.IsNull(mSut.Value);
        }

        [TestCase("type=\"\"")]
        [TestCase("")]
        public void Load_ShouldMaintainType_FromCtor(string type)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml($"<appender name=\"ColoredConsoleAppender\" {type}>\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(AppenderDescriptor.Async.TypeNamespace, mSut.Value);
        }

        [TestCase(null)]
        [TestCase("")]
        public void Save_ShouldNotSaveValueToAttribute_WhenValueIsNullOrEmpty(string value)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Value = value;

            mSut.Save(xmlDoc, appender);

            Assert.IsNull(appender.Attributes["type"]);
        }

        [Test]
        public void IsReadOnly_ShouldBeFalse_RegularCtor()
        {
            mSut = new TypeAttribute();

            Assert.IsFalse(mSut.IsReadOnly);
        }

        [Test]
        public void IsReadOnly_ShouldBeTrue_AppenderDescriptorCtor()
        {
            Assert.IsTrue(mSut.IsReadOnly);
        }

        [Test]
        public void Load_ShouldLoadCorrectType()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.Load(config);

            config.Received(1).Load("type", out _);
        }

        [Test]
        public void Name_ShouldBeCorrect_AppenderDescriptorCtor()
        {
            Assert.AreEqual("Type:", mSut.Name);
        }

        [Test]
        public void Name_ShouldBeCorrect_RegularCtor()
        {
            mSut = new TypeAttribute();

            Assert.AreEqual("Type:", mSut.Name);
        }

        [Test]
        public void Save_ShouldSaveValueToAttribute()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            const string value = "type";
            mSut.Value = value;

            mSut.Save(config);

            config.Received(1).Save("type", value);
        }

        [Test]
        public void Value_ShouldBeCorrect_AppenderDescriptorCtor()
        {
            Assert.AreEqual(AppenderDescriptor.Async.TypeNamespace, mSut.Value);
        }

        [Test]
        public void Value_ShouldBeCorrect_RegularCtor()
        {
            mSut = new TypeAttribute();

            Assert.IsNull(mSut.Value);
        }
    }
}
