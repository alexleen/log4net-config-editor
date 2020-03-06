// Copyright © 2018 Alex Leendertsen

using System.Xml;
using Editor.ConfigProperties;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class AditivityTest
    {
        [SetUp]
        public void SetUp()
        {
            mSut = new Aditivity();
        }

        private Aditivity mSut;

        [TestCase("<logger />", true)]
        [TestCase("<logger aditivity=\"\" />", true)]
        [TestCase("<logger aditivity=\"FALSE\" />", false)]
        [TestCase("<logger aditivity=\"False\" />", false)]
        [TestCase("<logger aditivity=\"false\" />", false)]
        public void Load_ShouldLoadTheCorrectValue(string xml, bool expected)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(expected, mSut.Value);
        }

        [Test]
        public void Name_ShouldBeInitializedCorrectly()
        {
            Assert.AreEqual("Aditivity:", mSut.Name);
        }

        [Test]
        public void Save_ShouldNotSaveIfAditive()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement logger = xmlDoc.CreateElement("logger");

            mSut.Save(xmlDoc, logger);

            CollectionAssert.IsEmpty(logger.Attributes);
        }

        [Test]
        public void Save_ShouldSaveIfNotAditive()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement logger = xmlDoc.CreateElement("logger");

            mSut.Value = false;
            mSut.Save(xmlDoc, logger);

            Assert.AreEqual("False", logger.Attributes?["aditivity"].Value);
        }

        [Test]
        public void Value_ShouldDefaultToTrue()
        {
            Assert.IsTrue(mSut.Value);
        }
    }
}
