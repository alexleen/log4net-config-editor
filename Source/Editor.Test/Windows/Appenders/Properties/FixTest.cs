// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using Editor.Windows.Appenders.Properties;
using Editor.Windows.PropertyCommon;
using log4net.Core;
using NUnit.Framework;

namespace Editor.Test.Windows.Appenders.Properties
{
    [TestFixture]
    public class FixTest
    {
        private Fix mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new Fix(new ObservableCollection<IProperty>());
        }

        [Test]
        public void Fixes_ShouldBeInitializedCorrectly()
        {
            CollectionAssert.AreEqual(Enum.GetValues(typeof(FixFlags)).Cast<FixFlags>(), mSut.Fixes);
        }

        [Test]
        public void SelectedFix_ShouldBeInitializedToNone()
        {
            Assert.AreEqual(FixFlags.None, mSut.SelectedFix);
        }

        [TestCase(null, FixFlags.None)]
        [TestCase("<Fix />", FixFlags.None)]
        [TestCase("<Fix value=\"\" />", FixFlags.None)]
        [TestCase("<Fix value=\"2\" />", FixFlags.Ndc)]
        [TestCase("<Fix value=\"200\" />", FixFlags.None)]
        [TestCase("<Fix value=\"str\" />", FixFlags.None)]
        public void Load_ShouldLoadCorrectly(string xml, FixFlags expected)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender name=\"asyncAppender\" type=\"Log4Net.Async.AsyncForwardingAppender,Log4Net.Async\">\n" +
                           $"      {xml}\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(expected, mSut.SelectedFix);
        }

        [Test]
        public void Save_ShouldSaveCorrectly()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Save(xmlDoc, appender);

            XmlNode fixNode = appender.SelectSingleNode("Fix");

            Assert.IsNotNull(fixNode);
            Assert.AreEqual("0", fixNode.Attributes?["value"].Value);
        }
    }
}
