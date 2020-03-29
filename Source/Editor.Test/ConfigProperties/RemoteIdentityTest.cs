// Copyright © 2018 Alex Leendertsen

using System.Xml;
using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Utilities;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class RemoteIdentityTest
    {
        [SetUp]
        public void SetUp()
        {
            mSut = new RemoteIdentity();
        }

        private RemoteIdentity mSut;

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
        public void Name_ShouldBeCorrect()
        {
            Assert.AreEqual("Identity:", mSut.Name);
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
            Assert.AreEqual(mSut.Value, identityNode.Attributes[Log4NetXmlConstants.Value].Value);
            Assert.AreEqual(LayoutDescriptor.Pattern.TypeNamespace, identityNode.Attributes[Log4NetXmlConstants.Type].Value);
        }

        [Test]
        public void ToolTip_ShouldBeCorrect()
        {
            Assert.AreEqual("Enter remote syslog identity pattern here.", mSut.ToolTip);
        }
    }
}
