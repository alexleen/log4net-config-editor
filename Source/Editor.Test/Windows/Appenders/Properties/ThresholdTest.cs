// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.Appenders.Properties;
using Editor.Windows.PropertyCommon;
using log4net.Core;
using NUnit.Framework;

namespace Editor.Test.Windows.Appenders.Properties
{
    [TestFixture]
    public class ThresholdTest
    {
        private Threshold mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new Threshold(new ObservableCollection<IProperty>());
        }

        [Test]
        public void Ctor_ShouldInitializeNameCorrectly()
        {
            Assert.AreEqual("Threshold:", mSut.Name);
        }

        [Test]
        public void Levels_ShouldBeAllLevels()
        {
            CollectionAssert.AreEqual(new[] { string.Empty }.Concat(Log4NetUtilities.LevelsByName.Keys), mSut.Levels);
        }

        [Test]
        public void SelectedLevel_ShouldBeNone()
        {
            Assert.AreEqual(string.Empty, mSut.SelectedLevel);
        }

        [TestCase("<threshold />", "")]
        [TestCase("<threshold value=\"\" />", "")]
        [TestCase("<threshold value=\"ALL\" />", "ALL")]
        [TestCase("<threshold value=\"All\" />", "ALL")]
        [TestCase("<threshold value=\"all\" />", "ALL")]
        public void Load_ShouldLoadTheCorrectValue(string xml, string expected)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\n" +
                        $"    {xml}\n" +
                        "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(expected, mSut.SelectedLevel);
        }

        [Test]
        public void Save_ShouldNotSave_WhenUnseleted()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Save(xmlDoc, appender);

            CollectionAssert.IsEmpty(appender.ChildNodes);
        }

        [Test]
        public void Save_ShouldSaveSelectedLevel()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.SelectedLevel = Level.All.Name;
            mSut.Save(xmlDoc, appender);

            XmlNode thresholdNode = appender.SelectSingleNode("threshold");

            Assert.IsNotNull(thresholdNode);
            Assert.AreEqual(Level.All.Name, thresholdNode.Attributes?["value"].Value);
        }
    }
}
