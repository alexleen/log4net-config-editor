// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Interfaces;
using Editor.Utilities;
using NUnit.Framework;
using static log4net.Appender.LocalSyslogAppender;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class LocalSyslogFacilityTest
    {
        private LocalSyslogFacility mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new LocalSyslogFacility(new ReadOnlyCollection<IProperty>(new List<IProperty>()));
        }

        [Test]
        public void RowHeight_ShouldBeCorrect()
        {
            Assert.AreEqual(mSut.RowHeight, GridLength.Auto);
        }

        [Test]
        public void Name_ShouldBeCorrect()
        {
            Assert.AreEqual("Facility:", mSut.Name);
        }

        [Test]
        public void Width_ShouldBeCorrect()
        {
            Assert.AreEqual(110, mSut.Width);
        }

        [Test]
        public void SelectedValue_ShouldBeInitializedToUser()
        {
            Assert.AreEqual(SyslogFacility.User.ToString(), mSut.SelectedValue);
        }

        [TestCase(null, "User")]
        [TestCase("<facility />", "User")]
        [TestCase("<facility value=\"\" />", "User")]
        [TestCase("<facility value=\"whatev\" />", "User")]
        [TestCase("<facility value=\"Alert\" />", "Alert")]
        public void Load_ShouldLoadCorrectly(string xml, string expected)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\n" +
                           $"   {xml}\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(expected, mSut.SelectedValue);
        }

        [Test]
        public void Save_ShouldSave()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appenderElement = xmlDoc.CreateElement("appender");

            mSut.SelectedValue = SyslogFacility.Alert.ToString();
            mSut.Save(xmlDoc, appenderElement);

            XmlNode node = appenderElement.SelectSingleNode(Log4NetXmlConstants.Facility);

            Assert.IsNotNull(node);
            Assert.AreEqual(mSut.SelectedValue, node.Attributes[Log4NetXmlConstants.Value].Value);
        }
    }
}
