// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Xml;
using Editor.Windows.Appenders.Properties;
using Editor.Windows.PropertyCommon;
using NUnit.Framework;

namespace Editor.Test.Windows.Appenders.Properties
{
    [TestFixture]
    public class TargetTest
    {
        private const string ConsoleOut = "Console.Out";
        private const string ConsoleError = "Console.Error";
        private Target mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new Target(new ObservableCollection<IProperty>());
        }

        [Test]
        public void Targets_ShouldBeInitializedCorrectly()
        {
            CollectionAssert.AreEqual(new[] { ConsoleOut, ConsoleError }, mSut.Targets);
        }

        [Test]
        public void SelectedTarget_ShouldBeInitializedToNone()
        {
            Assert.AreEqual(ConsoleOut, mSut.SelectedItem);
        }

        [TestCase(null, ConsoleOut)]
        [TestCase("<target />", ConsoleOut)]
        [TestCase("<target value=\"\" />", ConsoleOut)]
        [TestCase("<target value=\"whatev\" />", ConsoleOut)]
        [TestCase("<target value=\"Console.Error\" />", ConsoleError)]
        public void Load_ShouldLoadCorrectly(string xml, string expected)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\n" +
                           $"      {xml}\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(expected, mSut.SelectedItem);
        }

        [Test]
        public void Save_ShouldNotSaveIfConsoleOut()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Save(xmlDoc, appender);

            CollectionAssert.IsEmpty(appender.ChildNodes);
        }

        [Test]
        public void Save_ShouldSaveIfConsoleError()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.SelectedItem = ConsoleError;
            mSut.Save(xmlDoc, appender);

            XmlNode targetNode = appender.SelectSingleNode("target");

            Assert.IsNotNull(targetNode);
            Assert.AreEqual(ConsoleError, targetNode.Attributes?["value"].Value);
        }
    }
}
