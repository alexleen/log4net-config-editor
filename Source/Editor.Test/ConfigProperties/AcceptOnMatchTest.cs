// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Interfaces;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class AcceptOnMatchTest
    {
        private AcceptOnMatch mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new AcceptOnMatch(new ReadOnlyCollection<IProperty>(new List<IProperty>()));
        }

        [Test]
        public void AcceptOnMatch_ShouldUseCorrectName()
        {
            Assert.AreEqual("Accept on Match:", mSut.Name);
        }

        [Test]
        public void Name_ShouldBeInitializedCorrectly()
        {
            Assert.AreEqual("Accept on Match:", mSut.Name);
        }

        [Test]
        public void Accept_ShouldDefaultToTrue()
        {
            Assert.IsTrue(mSut.Value);
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

            Assert.AreEqual(expected, mSut.Value);
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

            mSut.Value = false;
            mSut.Save(xmlDoc, appender);

            XmlNode acceptNode = appender.SelectSingleNode("acceptOnMatch");

            Assert.IsNotNull(acceptNode);
            Assert.AreEqual("False", acceptNode.Attributes?["value"].Value);
        }
    }
}
