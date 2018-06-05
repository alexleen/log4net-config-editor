using Editor.Windows.Filters.Properties;
using Editor.Windows.PropertyCommon;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Xml;

namespace Editor.Test.Windows.Filters.Properties
{
    [TestFixture]
    public class StringMatchTest
    {
        private StringMatch mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new StringMatch(new ObservableCollection<IProperty>(), Validate);
        }

        private bool Validate()
        {
            return false;
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
