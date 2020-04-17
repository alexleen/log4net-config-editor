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
        private XmlElement mNewNode;

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

            mNewNode = xmlDoc.CreateElement("newAppender");
            mSut = new ElementConfiguration(xmlDoc, xmlDoc.FirstChild, xmlDoc.FirstChild.FirstChild, mNewNode);
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

        private static readonly IEnumerable<TestCaseData> sPropAttrTestValues = new[]
        {
            new TestCaseData("PROPERTY", "ATTR"),
            new TestCaseData("PrOpErTy", "AtTr"),
            new TestCaseData("Property", "Attr"),
            new TestCaseData("property", "attr")
        };

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

        [Test]
        public void Save_ShouldSaveAsNewElementWithAttributeValue()
        {
            const string elementName = "newElement";
            const string attributeName = "newAttr";
            const string attrValue = "attrValue";
            mSut.Save(elementName, attributeName, attrValue);

            Assert.AreEqual(attrValue, mNewNode[elementName].Attributes[attributeName].Value);
        }

        [Test]
        public void Save_ShouldSaveAsNewAttributeWithValue()
        {
            const string attributeName = "newAttr";
            const string attrValue = "attrValue";
            mSut.Save(attributeName, attrValue);

            Assert.AreEqual(attrValue, mNewNode.Attributes[attributeName].Value);
        }
    }
}
