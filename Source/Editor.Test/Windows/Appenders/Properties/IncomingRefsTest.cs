// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using Editor.Models;
using Editor.Utilities;
using Editor.Windows.Appenders.Properties;
using Editor.Windows.PropertyCommon;
using NUnit.Framework;

namespace Editor.Test.Windows.Appenders.Properties
{
    [TestFixture]
    public class IncomingRefsTest
    {
        private IncomingRefs mSut;
        private XmlDocument mXmlDoc;
        private Name mNameProperty;

        [SetUp]
        public void SetUp()
        {
            mXmlDoc = new XmlDocument();
            mXmlDoc.LoadXml("<log4net>\n" +
                            "  <appender name=\"appender0\">\n" +
                            "    <appender-ref ref=\"appender1\" />\n" +
                            "    <appender-ref ref=\"appender2\" />\n" +
                            "  </appender>\n" +
                            "  <appender name=\"appender1\">\n" +
                            "    <appender-ref ref=\"appender2\" />\n" +
                            "  </appender>\n" +
                            "  <appender name=\"appender2\">\n" +
                            "  </appender>\n" +
                            "  <appender name=\"appender3\">\n" +
                            "  </appender>\n" +
                            "  <appender name=\"asyncAppender\" type=\"Log4Net.Async.AsyncForwardingAppender,Log4Net.Async\">\n" +
                            "    <appender-ref ref=\"appender0\" />\n" +
                            "  </appender>\n" +
                            "  <root>\n" +
                            "    <appender-ref ref=\"asyncAppender\" />\n" +
                            "  </root>\n" +
                            "</log4net>");

            ObservableCollection<IProperty> properties = new ObservableCollection<IProperty>();
            mNameProperty = new Name(properties, mXmlDoc.FirstChild, mXmlDoc.FirstChild["appender"]);

            mSut = new IncomingRefs(mXmlDoc.FirstChild, mNameProperty, properties, mXmlDoc.FirstChild["appender"]);
        }

        [Test]
        public void Name_ShouldBeInitializedCorrectly()
        {
            Assert.AreEqual("↓ Refs:", mSut.Name);
        }

        [Test]
        public void Description_ShouldBeInitializedCorrectly()
        {
            Assert.AreEqual("This appender can be referenced in the following elements:", mSut.Description);
        }

        [Test]
        public void Ctor_ShouldLoadAvailableRefs()
        {
            Assert.AreEqual(2, mSut.RefsCollection.Count);
            Assert.IsTrue(mSut.RefsCollection.All(r => !r.IsEnabled)); //Locations aren't enabled until a load is done with this appender's name
            Assert.IsTrue(mSut.RefsCollection.All(r => r.LoggerNode.Name == "root" || r.LoggerNode.Attributes?["type"].Value == "Log4Net.Async.AsyncForwardingAppender,Log4Net.Async"));
        }

        [Test]
        public void Load_ShouldLoadEnabledRefs()
        {
            mNameProperty.Load(mXmlDoc.FirstChild["appender"]);
            mSut.Load(mXmlDoc.FirstChild["appender"]);

            Assert.AreEqual(2, mSut.RefsCollection.Count);
            Assert.AreEqual(1, mSut.RefsCollection.Count(r => !r.IsEnabled));
            Assert.IsTrue(mSut.RefsCollection.All(r => r.LoggerNode.Name == "root" || r.LoggerNode.Attributes?["type"].Value == "Log4Net.Async.AsyncForwardingAppender,Log4Net.Async"));
        }

        [Test]
        public void Load_ShouldNotLoadRefToItself()
        {
            mXmlDoc = new XmlDocument();
            mXmlDoc.LoadXml("<log4net>\n" +
                            "  <appender name=\"asyncAppender\" type=\"Log4Net.Async.AsyncForwardingAppender,Log4Net.Async\">\n" +
                            "    <appender-ref ref=\"appender0\" />\n" +
                            "  </appender>\n" +
                            "  <appender name=\"appender0\">\n" +
                            "    <appender-ref ref=\"appender1\" />\n" +
                            "    <appender-ref ref=\"appender2\" />\n" +
                            "  </appender>\n" +
                            "  <appender name=\"appender1\">\n" +
                            "    <appender-ref ref=\"appender2\" />\n" +
                            "  </appender>\n" +
                            "  <appender name=\"appender2\">\n" +
                            "  </appender>\n" +
                            "  <appender name=\"appender3\">\n" +
                            "  </appender>\n" +
                            "  <root>\n" +
                            "    <appender-ref ref=\"asyncAppender\" />\n" +
                            "  </root>\n" +
                            "</log4net>");

            ObservableCollection<IProperty> properties = new ObservableCollection<IProperty>();
            mNameProperty = new Name(properties, mXmlDoc.FirstChild, mXmlDoc.FirstChild["appender"]);

            mSut = new IncomingRefs(mXmlDoc.FirstChild, mNameProperty, properties, mXmlDoc.FirstChild["appender"]);

            Assert.AreEqual(1, mSut.RefsCollection.Count);
            Assert.IsTrue(mSut.RefsCollection.All(r => !r.IsEnabled)); //Locations aren't enabled until a load is done with this appender's name
            Assert.IsTrue(mSut.RefsCollection.All(r => r.LoggerNode.Name == "root"));
        }

        [Test]
        public void Save_ShouldSaveRef_WhenNoneExist()
        {
            XmlElement loggerElement = mXmlDoc.CreateElement("logger");

            mSut.RefsCollection = new ObservableCollection<LoggerModel>
            {
                new LoggerModel("logger", loggerElement, true)
            };

            mSut.Save(mXmlDoc, mXmlDoc.CreateElement("appender"));

            XmlNodeList appenderRefs = loggerElement.SelectNodes($"appender-ref[@ref='{mNameProperty.Value}']");

            Assert.IsNotNull(appenderRefs);
            Assert.AreEqual(1, appenderRefs.Count);
        }

        [Test]
        public void Save_ShouldNotDuplicateExistingRef()
        {
            XmlElement loggerElement = mXmlDoc.CreateElement("logger");
            mXmlDoc.CreateElementWithAttribute("appender-ref", "ref", mNameProperty.Value).AppendTo(loggerElement);

            mSut.RefsCollection = new ObservableCollection<LoggerModel>
            {
                new LoggerModel("logger", loggerElement, true)
            };

            mSut.Save(mXmlDoc, mXmlDoc.CreateElement("appender"));

            XmlNodeList appenderRefs = loggerElement.SelectNodes($"appender-ref[@ref='{mNameProperty.Value}']");

            Assert.IsNotNull(appenderRefs);
            Assert.AreEqual(1, appenderRefs.Count);
        }

        [Test]
        public void Save_ShouldReduceRefCountToOne()
        {
            XmlElement loggerElement = mXmlDoc.CreateElement("logger");
            mXmlDoc.CreateElementWithAttribute("appender-ref", "ref", mNameProperty.Value).AppendTo(loggerElement);
            mXmlDoc.CreateElementWithAttribute("appender-ref", "ref", mNameProperty.Value).AppendTo(loggerElement);

            mSut.RefsCollection = new ObservableCollection<LoggerModel>
            {
                new LoggerModel("logger", loggerElement, true)
            };

            mSut.Save(mXmlDoc, mXmlDoc.CreateElement("appender"));

            XmlNodeList appenderRefs = loggerElement.SelectNodes($"appender-ref[@ref='{mNameProperty.Value}']");

            Assert.IsNotNull(appenderRefs);
            Assert.AreEqual(1, appenderRefs.Count);
        }

        [Test]
        public void Save_ShouldRemoveExistingRefs_WhenNotEnabled()
        {
            XmlElement loggerElement = mXmlDoc.CreateElement("logger");
            mXmlDoc.CreateElementWithAttribute("appender-ref", "ref", mNameProperty.Value).AppendTo(loggerElement);
            mXmlDoc.CreateElementWithAttribute("appender-ref", "ref", mNameProperty.Value).AppendTo(loggerElement);

            mSut.RefsCollection = new ObservableCollection<LoggerModel>
            {
                new LoggerModel("logger", loggerElement, false)
            };

            mSut.Save(mXmlDoc, mXmlDoc.CreateElement("appender"));

            XmlNodeList appenderRefs = loggerElement.SelectNodes($"appender-ref[@ref='{mNameProperty.Value}']");

            Assert.IsNotNull(appenderRefs);
            Assert.AreEqual(0, appenderRefs.Count);
        }

        [Test]
        public void Save_ShouldNotAddRef_WhenNotEnabled()
        {
            XmlElement loggerElement = mXmlDoc.CreateElement("logger");

            mSut.RefsCollection = new ObservableCollection<LoggerModel>
            {
                new LoggerModel("logger", loggerElement, false)
            };

            mSut.Save(mXmlDoc, mXmlDoc.CreateElement("appender"));

            XmlNodeList appenderRefs = loggerElement.SelectNodes($"appender-ref[@ref='{mNameProperty.Value}']");

            Assert.IsNotNull(appenderRefs);
            Assert.AreEqual(0, appenderRefs.Count);
        }
    }
}
