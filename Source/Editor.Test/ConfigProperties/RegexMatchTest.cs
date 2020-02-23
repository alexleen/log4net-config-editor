// Copyright © 2018 Alex Leendertsen

using System.Xml;
using Editor.ConfigProperties;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class RegexMatchTest
    {
        [SetUp]
        public void SetUp()
        {
            mValidateCalled = false;
            mSut = new RegexMatch(Validate);
        }

        private bool mValidateCalled;
        private RegexMatch mSut;

        private bool Validate()
        {
            mValidateCalled = true;
            return false;
        }

        [TestCase(null, null)]
        [TestCase("<regexToMatch />", null)]
        [TestCase("<regexToMatch value=\"\" />", null)]
        [TestCase("<regexToMatch value=\"str\" />", "str")]
        public void Load_ShouldLoadCorrectly(string xml, string expected)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\n" +
                           $"      {xml}\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(expected, mSut.Value);
        }

        [TestCase(null)]
        [TestCase("")]
        public void Save_ShouldNotSave_WhenNullOrEmptyValue(string value)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement filterElement = xmlDoc.CreateElement("filter");

            mSut.Value = value;
            mSut.Save(xmlDoc, filterElement);

            CollectionAssert.IsEmpty(filterElement.ChildNodes);
        }

        [Test]
        public void Save_ShouldSave()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement filterElement = xmlDoc.CreateElement("filter");

            mSut.Value = "match";
            mSut.Save(xmlDoc, filterElement);

            XmlNode regexNode = filterElement.SelectSingleNode("regexToMatch");

            Assert.IsNotNull(regexNode);
            Assert.AreEqual(mSut.Value, regexNode.Attributes["value"].Value);
        }

        [Test]
        public void TryValidate_ShouldCallValidate()
        {
            //Test sanity check
            Assert.IsFalse(mValidateCalled);

            mSut.TryValidate(Substitute.For<IMessageBoxService>());

            Assert.IsTrue(mValidateCalled);
        }
    }
}
