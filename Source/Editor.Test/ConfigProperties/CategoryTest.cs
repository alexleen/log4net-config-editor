// Copyright © 2020 Alex Leendertsen

using System.Xml;
using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Utilities;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class CategoryTest
    {
        private const string DefaultValue = "%logger";
        private Category mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new Category();
        }

        [Test]
        public void Name_ShouldBeInitializedCorrectly()
        {
            Assert.AreEqual("Category:", mSut.Name);
        }

        [Test]
        public void Value_ShouldBeInitializedToDefault()
        {
            Assert.AreEqual(DefaultValue, mSut.Value);
        }

        [Test]
        public void Load_ShouldLoadCorrectValue()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>" +
                           "  <category type=\"log4net.Layout.PatternLayout\">" +
                           "    <conversionPattern value=\"%logger %date\" />" +
                           "  </category>" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual("%logger %date", mSut.Value);
        }

        [Test]
        public void Load_ShouldNotLoad_WhenNoCategoryExists()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(DefaultValue, mSut.Value);
        }

        [Test]
        public void Load_ShouldNotLoad_WhenNoConversionPatternExists()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>" +
                           "  <category type=\"log4net.Layout.PatternLayout\">" +
                           "  </category>" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(DefaultValue, mSut.Value);
        }

        [Test]
        public void Load_ShouldNotLoad_WhenEmptyValue()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>" +
                           "  <category type=\"log4net.Layout.PatternLayout\">" +
                           "    <conversionPattern value=\"\" />" +
                           "  </category>" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(DefaultValue, mSut.Value);
        }

        [Test]
        public void Save_ShouldSaveCorrectValue()
        {
            mSut.Value = "whatev";

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Save(xmlDoc, appender);

            XmlElement category = appender["category"];
            Assert.IsNotNull(category);
            Assert.AreEqual(LayoutDescriptor.Pattern.TypeNamespace, category.Attributes[Log4NetXmlConstants.Type].Value);

            XmlElement conversionPattern = category["conversionPattern"];
            Assert.IsNotNull(conversionPattern);
            Assert.AreEqual("whatev", conversionPattern.Attributes[Log4NetXmlConstants.Value].Value);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("%logger")]
        public void Save_ShouldNotSave(string value)
        {
            mSut.Value = value;

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Save(xmlDoc, appender);

            CollectionAssert.IsEmpty(appender.ChildNodes);
        }
    }
}
