// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.Appenders.Properties;
using Editor.Windows.PropertyCommon;
using NUnit.Framework;

namespace Editor.Test.Windows.Appenders.Properties
{
    [TestFixture]
    public class CountDirectionTest
    {
        private const string Lower = "Lower";
        private const string Higher = "Higher";
        private const string CountDirectionName = "countDirection";
        private CountDirection mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new CountDirection(new ObservableCollection<IProperty>());
        }

        [Test]
        public void Ctor_ShouldInitDirectionsCorrectly()
        {
            CollectionAssert.AreEquivalent(new[] { Lower, Higher }, mSut.Directions);
        }

        [Test]
        public void Ctor_ShouldInitDefaultToLower()
        {
            Assert.AreEqual(Lower, mSut.SelectedDirection);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("string")]
        public void Load_ShouldSetDefaultValue_WhenAttributeValueIsNotAnInt(string value)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\r\n" +
                           $"    <{CountDirectionName} value=\"{value}\" />\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(Lower, mSut.SelectedDirection);
        }

        [TestCase(-1, Lower)]
        [TestCase(0, Higher)]
        [TestCase(1, Higher)]
        public void Load_ShouldSetCorrectValue(int directionInt, string directionStr)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\r\n" +
                           $"    <{CountDirectionName} value=\"{directionInt}\" />\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(directionStr, mSut.SelectedDirection);
        }

        [Test]
        public void Save_ShouldNotSaveIfNotHigher()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement parent = xmlDoc.CreateElement("element");

            mSut.Save(xmlDoc, parent);

            Assert.IsNull(parent[CountDirectionName]);
        }

        [Test]
        public void Save_ShouldSaveIfHigher()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement parent = xmlDoc.CreateElement("element");

            mSut.SelectedDirection = Higher;

            mSut.Save(xmlDoc, parent);

            Assert.AreEqual("0", parent.GetValueAttributeValueFromChildElement(CountDirectionName));
        }
    }
}
