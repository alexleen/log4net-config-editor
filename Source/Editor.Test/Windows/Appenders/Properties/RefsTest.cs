// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Xml;
using Editor.Models;
using Editor.Utilities;
using Editor.Windows.Appenders.Properties;
using Editor.Windows.PropertyCommon;
using NUnit.Framework;

namespace Editor.Test.Windows.Appenders.Properties
{
    [TestFixture]
    public class RefsTest
    {
        private Refs mSut;
        private XmlDocument mXmlDoc;
        private Name mNameProperty;

        [SetUp]
        public void SetUp()
        {
            mXmlDoc = new XmlDocument();
            XmlElement log4NetElement = mXmlDoc.CreateElement("log4net");
            XmlElement originalAppender = mXmlDoc.CreateElement("appender");
            ObservableCollection<IProperty> properties = new ObservableCollection<IProperty>();
            mNameProperty = new Name(properties, log4NetElement, originalAppender);
            mNameProperty.Value = "appenderName";

            mSut = new Refs(log4NetElement, mNameProperty, properties);
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
