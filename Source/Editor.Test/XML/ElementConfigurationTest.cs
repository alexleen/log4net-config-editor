// Copyright © 2020 Alex Leendertsen

using System.Collections.Generic;
using System.Xml;
using Editor.Interfaces;
using Editor.XML;
using NUnit.Framework;

namespace Editor.Test.XML
{
    [TestFixture]
    public class ElementConfigurationTest
    {
        private IElementConfiguration mSut;

        [SetUp]
        public void SetUp()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<log4net>" +
                           "  <appender type=\"some.type\">" +
                           "    <property attr =\"someValue\"/>" +
                           "  </appender>" +
                           "</log4net>");

            mSut = new ElementConfiguration(xmlDoc, xmlDoc.FirstChild, xmlDoc.FirstChild.FirstChild, xmlDoc.CreateElement("newAppender"));
        }

        [Test]
        public void Load_ShouldNotLoad_WhenNoSuchElementExists()
        {
            Assert.IsFalse(mSut.Load("whatev", "attr", out IValueResult result));
            Assert.IsNull(result);
        }

        [Test]
        public void Load_ShouldNotLoad_WhenNoSuchAttributeExists()
        {
            Assert.IsFalse(mSut.Load("property", "whatev", out IValueResult result));
            Assert.IsNull(result);
        }

        private static readonly IEnumerable<TestCaseData> sPropAttrTestValues = new[] { new TestCaseData("PROPERTY", "ATTR"), new TestCaseData("Property", "Attr"), new TestCaseData("property", "attr") };

        [TestCaseSource(nameof(sPropAttrTestValues))]
        public void Load_ShouldLoadCorrectValue(string elementName, string attributeName)
        {
            Assert.IsTrue(mSut.Load(elementName, attributeName, out IValueResult result));
            Assert.AreEqual("someValue", result.AttributeValue);
        }

        [TestCaseSource(nameof(sPropAttrTestValues))]
        public void Load_ShouldReturnOriginalElementName(string elementName, string attributeName)
        {
            Assert.IsTrue(mSut.Load(elementName, attributeName, out IValueResult result));
            Assert.AreEqual("property", result.ActualElementName);
        }

        [TestCaseSource(nameof(sPropAttrTestValues))]
        public void Load_ShouldReturnOriginalAttributeName(string elementName, string attributeName)
        {
            Assert.IsTrue(mSut.Load(elementName, attributeName, out IValueResult result));
            Assert.AreEqual("attr", result.ActualAttributeName);
        }
    }
}
