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
    public class LocalIdentityTest
    {
        private LocalIdentity mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new LocalIdentity(new ReadOnlyCollection<IProperty>(new List<IProperty>()));
        }

        [Test]
        public void Name_ShouldBeCorrect()
        {
            Assert.AreEqual("Identity:", mSut.Name);
        }

        [TestCase(null, null)]
        [TestCase("<identity />", null)]
        [TestCase("<identity value=\"\" />", null)]
        [TestCase("<identity value=\"str\" />", "str")]
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
            XmlElement appenderElement = xmlDoc.CreateElement("appender");

            mSut.Value = value;
            mSut.Save(xmlDoc, appenderElement);

            CollectionAssert.IsEmpty(appenderElement.ChildNodes);
        }

        [Test]
        public void Save_ShouldSave()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appenderElement = xmlDoc.CreateElement("appender");

            mSut.Value = "str";
            mSut.Save(xmlDoc, appenderElement);

            XmlNode identityNode = appenderElement.SelectSingleNode("identity");

            Assert.IsNotNull(identityNode);
            Assert.AreEqual(mSut.Value, identityNode.Attributes["value"].Value);
        }
    }
}
