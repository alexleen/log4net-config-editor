// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Xml;
using Editor.Windows;
using Editor.Windows.Filters.Properties;
using Editor.Windows.PropertyCommon;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.Windows.Filters.Properties
{
    [TestFixture]
    public class StringMatchTest
    {
        private bool mValidateCalled;
        private StringMatch mSut;

        [SetUp]
        public void SetUp()
        {
            mValidateCalled = false;
            mSut = new StringMatch(new ObservableCollection<IProperty>(), Validate);
        }

        private bool Validate()
        {
            mValidateCalled = true;
            return false;
        }

        [TestCase(null, null)]
        [TestCase("<stringToMatch />", null)]
        [TestCase("<stringToMatch value=\"\" />", null)]
        [TestCase("<stringToMatch value=\"str\" />", "str")]
        public void Load_ShouldLoadCorrectly(string xml, string expected)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\n" +
                           $"      {xml}\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(expected, mSut.Value);
        }

        [Test]
        public void TryValidate_ShouldCallValidate()
        {
            //Test sanity check
            Assert.IsFalse(mValidateCalled);

            mSut.TryValidate(Substitute.For<IMessageBoxService>());

            Assert.IsTrue(mValidateCalled);
        }

        [TestCase(null)]
        [TestCase("")]
        public void Save_ShouldNotSaveStringMatch(string value)
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

            XmlNode stringNode = filterElement.SelectSingleNode("stringToMatch");

            Assert.IsNotNull(stringNode);
            Assert.AreEqual(mSut.Value, stringNode.Attributes["value"].Value);
        }
    }
}
