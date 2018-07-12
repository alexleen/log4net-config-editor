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
    public class StaticLogFileNameTest
    {
        private StaticLogFileName mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new StaticLogFileName(new ReadOnlyCollection<IProperty>(new List<IProperty>()));
        }

        [Test]
        public void Name_ShouldBeInitializedCorrectly()
        {
            Assert.AreEqual("Static Log File Name:", mSut.Name);
        }

        [Test]
        public void Value_ShouldBeFalseByDefault()
        {
            Assert.IsFalse(mSut.Value);
        }

        [TestCase("<staticLogFileName value=\"true\" />", true)]
        [TestCase("<staticLogFileName value=\"True\" />", true)]
        [TestCase("<staticLogFileName value=\"TRUE\" />", true)]
        [TestCase("<staticLogFileName value=\"false\" />", false)]
        [TestCase("<staticLogFileName value=\"False\" />", false)]
        [TestCase("<staticLogFileName value=\"FALSE\" />", false)]
        [TestCase("<staticLogFileName />", false)]
        [TestCase("<staticLogFileName value=\"\" />", false)]
        public void Load_ShouldLoadValue(string xml, bool expected)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender type=\"log4net.Appender.FileAppender\" name=\"file\">\n" +
                           $"    {xml}\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(expected, mSut.Value);
        }

        [Test]
        public void Save_ShouldNotSaveIfNotValue()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Save(xmlDoc, appender);

            CollectionAssert.IsEmpty(appender.ChildNodes);
        }

        [Test]
        public void Save_ShouldSaveIfValue()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Value = true;
            mSut.Save(xmlDoc, appender);

            XmlNode staticNode = appender.SelectSingleNode("staticLogFileName");

            Assert.IsNotNull(staticNode);
            Assert.AreEqual("True", staticNode.Attributes?["value"].Value);
        }
    }
}
