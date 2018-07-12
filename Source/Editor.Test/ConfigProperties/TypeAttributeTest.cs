// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class TypeAttributeTest
    {
        private TypeAttribute mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new TypeAttribute(new ReadOnlyCollection<IProperty>(new List<IProperty>()), AppenderDescriptor.Async);
        }

        [Test]
        public void Name_ShouldBeCorrect()
        {
            Assert.AreEqual("Type:", mSut.Name);
        }

        [Test]
        public void Value_ShouldBeCorrect()
        {
            Assert.AreEqual(AppenderDescriptor.Async.TypeNamespace, mSut.Value);
        }

        [Test]
        public void IsReadOnly_ShouldBeTrue()
        {
            Assert.IsTrue(mSut.IsReadOnly);
        }

        [Test]
        public void Load_ShouldLoadCorrectType()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender name=\"ColoredConsoleAppender\" type=\"log4net.Appender.ColoredConsoleAppender\">\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual("log4net.Appender.ColoredConsoleAppender", mSut.Value);
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

        [Test]
        public void Save_ShouldSaveNameToAttribute()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            const string nameValue = "type";
            mSut.Value = nameValue;

            mSut.Save(xmlDoc, appender);

            Assert.AreEqual(nameValue, appender.Attributes["type"].Value);
        }
    }
}
