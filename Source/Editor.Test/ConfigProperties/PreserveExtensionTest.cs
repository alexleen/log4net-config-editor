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
    public class PreserveExtensionTest
    {
        private PreserveExtension mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new PreserveExtension(new ReadOnlyCollection<IProperty>(new List<IProperty>()));
        }

        [Test]
        public void Preserve_ShouldBeFalseByDefault()
        {
            Assert.IsFalse(mSut.Preserve);
        }

        [TestCase("<preserveLogFileNameExtension value=\"true\" />", true)]
        [TestCase("<preserveLogFileNameExtension value=\"True\" />", true)]
        [TestCase("<preserveLogFileNameExtension value=\"TRUE\" />", true)]
        [TestCase("<preserveLogFileNameExtension value=\"false\" />", false)]
        [TestCase("<preserveLogFileNameExtension value=\"False\" />", false)]
        [TestCase("<preserveLogFileNameExtension value=\"FALSE\" />", false)]
        [TestCase("<preserveLogFileNameExtension />", false)]
        [TestCase("<preserveLogFileNameExtension value=\"\" />", false)]
        public void Load_ShouldLoadPreserve(string xml, bool expected)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender type=\"log4net.Appender.FileAppender\" name=\"file\">\n" +
                           $"    {xml}\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(expected, mSut.Preserve);
        }

        [Test]
        public void Save_ShouldNotSaveIfNotPreserve()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Save(xmlDoc, appender);

            CollectionAssert.IsEmpty(appender.ChildNodes);
        }

        [Test]
        public void Save_ShouldSaveIfPreserve()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Preserve = true;
            mSut.Save(xmlDoc, appender);

            XmlNode preserveNode = appender.SelectSingleNode("preserveLogFileNameExtension");

            Assert.IsNotNull(preserveNode);
            Assert.AreEqual("true", preserveNode.Attributes?["value"].Value);
        }
    }
}
