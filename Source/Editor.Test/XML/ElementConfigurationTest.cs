// Copyright © 2020 Alex Leendertsen

using System.Collections.Generic;
using System.Linq;
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
                           "    <property attr =\"someValue\">" +
                           "      <childProp childAttr =\"childValue\"/>" +
                           "    </property>" +
                           "  </appender>" +
                           "</log4net>");

            mNewNode = xmlDoc.CreateElement("newAppender");
            mSut = new ElementConfiguration(xmlDoc, xmlDoc.FirstChild, xmlDoc.FirstChild.FirstChild, mNewNode);
        }

        [Test]
        public void Load_ShouldNotLoad_WhenNoSuchElementExists()
        {
            Assert.IsFalse(mSut.Load("attr", out IValueResult result, "whatev"));
            Assert.IsNull(result);
        }

        [Test]
        public void Load_ShouldNotLoad_WhenNoSuchAttributeExists()
        {
            Assert.IsFalse(mSut.Load("whatev", out IValueResult result, "property"));
            Assert.IsNull(result);
        }

        [TestCase("TYPE")]
        [TestCase("TyPe")]
        [TestCase("Type")]
        [TestCase("type")]
        public void Load_ShouldOriginalNodeAttribute(string attributeName)
        {
            Assert.IsTrue(mSut.Load(attributeName, out IValueResult result));
            Assert.AreEqual("some.type", result.AttributeValue);
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
            Assert.IsTrue(mSut.Load(attributeName, out IValueResult result, elementName));
            Assert.AreEqual("someValue", result.AttributeValue);
        }

        [Test]
        public void Load_ShouldChildOfChild()
        {
            Assert.IsTrue(mSut.Load("childAttr", out IValueResult result, "property", "childProp"));
            Assert.AreEqual("childValue", result.AttributeValue);
        }

        [TestCaseSource(nameof(sPropAttrTestValues))]
        public void Load_ShouldReturnOriginalElementName(string elementName, string attributeName)
        {
            Assert.IsTrue(mSut.Load(attributeName, out IValueResult result, elementName));
            Assert.AreEqual("property", result.ActualElementNames.Single());
        }

        [TestCaseSource(nameof(sPropAttrTestValues))]
        public void Load_ShouldReturnOriginalAttributeName(string elementName, string attributeName)
        {
            Assert.IsTrue(mSut.Load(attributeName, out IValueResult result, elementName));
            Assert.AreEqual("attr", result.ActualAttributeName);
        }

        // Save ------------------------------------------------------------------------------------------

        [Test]
        public void Save_ShouldSaveMultipleChildren()
        {
            const string firstChild = "child1";
            const string firstAttr = "attr1";
            const string firstValue = "value1";
            const string secondChild = "child2";
            const string attributeName = "newAttr";
            const string attrValue = "attrValue";

            mSut.Save((firstChild, firstAttr, firstValue), (secondChild, attributeName, attrValue));

            Assert.AreEqual(firstValue, mNewNode[firstChild]?.Attributes[firstAttr].Value);
            Assert.AreEqual(attrValue, mNewNode[firstChild]?[secondChild]?.Attributes[attributeName].Value);
        }

        [Test]
        public void Save_ShouldSaveAsNewElementWithAttributeValue()
        {
            const string elementName = "newElement";
            const string attributeName = "newAttr";
            const string attrValue = "attrValue";
            mSut.Save((elementName, attributeName, attrValue));

            Assert.AreEqual(attrValue, mNewNode[elementName]?.Attributes[attributeName].Value);
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
