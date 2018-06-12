// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Xml;
using Editor.Windows.Filters.Properties;
using Editor.Windows.PropertyCommon;
using NUnit.Framework;

namespace Editor.Test.Windows.Filters.Properties
{
    [TestFixture]
    public class AcceptOnMatchTest
    {
        private AcceptOnMatch mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new AcceptOnMatch(new ObservableCollection<IProperty>());
        }

        [Test]
        public void Accept_ShouldDefaultToTrue()
        {
            Assert.IsTrue(mSut.Accept);
        }

        [TestCase("<acceptOnMatch />", true)]
        [TestCase("<acceptOnMatch value=\"\" />", true)]
        [TestCase("<acceptOnMatch value=\"FALSE\" />", false)]
        [TestCase("<acceptOnMatch value=\"False\" />", false)]
        [TestCase("<acceptOnMatch value=\"false\" />", false)]
        public void Load_ShouldLoadTheCorrectValue(string xml, bool expected)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\n" +
                           $"    {xml}\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(expected, mSut.Accept);
        }

        [Test]
        public void Save_ShouldNotSaveIfAccept()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Save(xmlDoc, appender);

            CollectionAssert.IsEmpty(appender.ChildNodes);
        }

        [Test]
        public void Save_ShouldSaveIfNotAccept()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Accept = false;
            mSut.Save(xmlDoc, appender);

            XmlNode acceptNode = appender.SelectSingleNode("acceptOnMatch");

            Assert.IsNotNull(acceptNode);
            Assert.AreEqual("false", acceptNode.Attributes?["value"].Value);
        }
    }
}
