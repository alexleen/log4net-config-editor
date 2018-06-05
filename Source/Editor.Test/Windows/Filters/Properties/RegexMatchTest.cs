using Editor.Windows.Filters.Properties;
using Editor.Windows.PropertyCommon;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Xml;

namespace Editor.Test.Windows.Filters.Properties
{
    [TestFixture]
    public class RegexMatchTest
    {
        private RegexMatch mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new RegexMatch(new ObservableCollection<IProperty>(), Validate);
        }

        private bool Validate()
        {
            return false;
        }

        [TestCase(null)]
        [TestCase("")]
        public void Save_ShouldNotSave_WhenNullOrEmtpyValue(string value)
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
    }
}
