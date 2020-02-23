// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Models.Base;
using Editor.Models.ConfigChildren;
using Editor.Utilities;
using Editor.XML;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class IncomingRefsTest
    {
        [SetUp]
        public void SetUp()
        {
            const string xml = "<log4net>\n" +
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
                               "</log4net>";

            mXmlDoc = new XmlDocument();

            ReadOnlyCollection<IProperty> properties = new ReadOnlyCollection<IProperty>(new List<IProperty>());
            IElementConfiguration appenderConfiguration = GetAppenderConfiguration(mXmlDoc, xml);
            mNameProperty = new Name(appenderConfiguration);
            mNameProperty.Load(appenderConfiguration.OriginalNode);

            mSut = new IncomingRefs(mNameProperty, appenderConfiguration);
        }

        private IncomingRefs mSut;
        private XmlDocument mXmlDoc;
        private Name mNameProperty;

        private IElementConfiguration GetAppenderConfiguration(XmlDocument xmlDoc, string xml)
        {
            xmlDoc.LoadXml(xml);

            IElementConfiguration appenderConfiguration = new ElementConfiguration(xmlDoc, xmlDoc.FirstChild, xmlDoc.FirstChild["appender"], null);

            return appenderConfiguration;
        }

        [Test]
        public void Ctor_ShouldLoadAvailableRefs()
        {
            Assert.AreEqual(2, mSut.RefsCollection.Count);
            Assert.IsTrue(mSut.RefsCollection.All(r => !r.IsEnabled)); //Locations aren't enabled until a load is done with this appender's name
            Assert.IsTrue(mSut.RefsCollection.All(r => r.Node.Name == "root" || r.Node.Attributes?["type"].Value == "Log4Net.Async.AsyncForwardingAppender,Log4Net.Async"));
        }

        [Test]
        public void Description_ShouldBeInitializedCorrectly()
        {
            Assert.AreEqual("This appender can be referenced in the following elements:", mSut.Description);
        }

        [Test]
        public void Load_ShouldLoadEnabledRefs()
        {
            mNameProperty.Load(mXmlDoc.FirstChild["appender"]);
            mSut.Load(mXmlDoc.FirstChild["appender"]);

            Assert.AreEqual(2, mSut.RefsCollection.Count);
            Assert.AreEqual(1, mSut.RefsCollection.Count(r => !r.IsEnabled));
            Assert.IsTrue(mSut.RefsCollection.All(r => r.Node.Name == "root" || r.Node.Attributes?["type"].Value == "Log4Net.Async.AsyncForwardingAppender,Log4Net.Async"));
        }

        [Test]
        public void Load_ShouldNotLoadAppender_WithNoName()
        {
            const string xml = "<log4net>\n" +
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
                               "  <appender type=\"Log4Net.Async.AsyncForwardingAppender,Log4Net.Async\">\n" +
                               "    <appender-ref ref=\"appender0\" />\n" +
                               "  </appender>\n" +
                               "</log4net>";

            ReadOnlyCollection<IProperty> properties = new ReadOnlyCollection<IProperty>(new List<IProperty>());
            IElementConfiguration appenderConfiguration = GetAppenderConfiguration(mXmlDoc, xml);
            mNameProperty = new Name(appenderConfiguration);

            mSut = new IncomingRefs(mNameProperty, appenderConfiguration);

            Assert.AreEqual(0, mSut.RefsCollection.Count);
        }

        [Test]
        public void Load_ShouldNotLoadRefToItself()
        {
            const string xml = "<log4net>\n" +
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
                               "</log4net>";

            ReadOnlyCollection<IProperty> properties = new ReadOnlyCollection<IProperty>(new List<IProperty>());
            IElementConfiguration appenderConfiguration = GetAppenderConfiguration(mXmlDoc, xml);
            mNameProperty = new Name(appenderConfiguration);

            mSut = new IncomingRefs(mNameProperty, appenderConfiguration);

            Assert.AreEqual(1, mSut.RefsCollection.Count);
            Assert.IsTrue(mSut.RefsCollection.All(r => !r.IsEnabled)); //Locations aren't enabled until a load is done with this appender's name
            Assert.IsTrue(mSut.RefsCollection.All(r => r.Node.Name == "root"));
        }

        [Test]
        public void Load_ShouldNotLoadRoot_WhenNonexistent()
        {
            const string xml = "<log4net>\n" +
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
                               "</log4net>";

            ReadOnlyCollection<IProperty> properties = new ReadOnlyCollection<IProperty>(new List<IProperty>());
            IElementConfiguration appenderConfiguration = GetAppenderConfiguration(mXmlDoc, xml);
            mNameProperty = new Name(appenderConfiguration);

            mSut = new IncomingRefs(mNameProperty, appenderConfiguration);

            Assert.AreEqual(1, mSut.RefsCollection.Count);
            Assert.IsTrue(mSut.RefsCollection.All(r => !r.IsEnabled)); //Locations aren't enabled until a load is done with this appender's name
            Assert.IsTrue(mSut.RefsCollection.All(r => r.Node.Name != "root"));
        }

        [Test]
        public void Name_ShouldBeInitializedCorrectly()
        {
            Assert.AreEqual("↓ Refs:", mSut.Name);
        }

        [Test]
        public void Save_ShouldNotAddRef_WhenNotEnabled()
        {
            XmlElement loggerElement = mXmlDoc.CreateElement("logger");

            mSut.RefsCollection = new ObservableCollection<IAcceptAppenderRef>
            {
                new LoggerModel(loggerElement, false, LoggerDescriptor.Logger)
            };

            mSut.Save(mXmlDoc, mXmlDoc.CreateElement("appender"));

            XmlNodeList appenderRefs = loggerElement.SelectNodes($"appender-ref[@ref='{mNameProperty.Value}']");

            Assert.IsNotNull(appenderRefs);
            Assert.AreEqual(0, appenderRefs.Count);
        }

        [Test]
        public void Save_ShouldNotDuplicateExistingRef()
        {
            XmlElement loggerElement = mXmlDoc.CreateElement("logger");
            mXmlDoc.CreateElementWithAttribute("appender-ref", "ref", mNameProperty.Value).AppendTo(loggerElement);

            mSut.RefsCollection = new ObservableCollection<IAcceptAppenderRef>
            {
                new LoggerModel(loggerElement, true, LoggerDescriptor.Logger)
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

            mSut.RefsCollection = new ObservableCollection<IAcceptAppenderRef>
            {
                new LoggerModel(loggerElement, true, LoggerDescriptor.Logger)
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

            mSut.RefsCollection = new ObservableCollection<IAcceptAppenderRef>
            {
                new LoggerModel(loggerElement, false, LoggerDescriptor.Logger)
            };

            mSut.Save(mXmlDoc, mXmlDoc.CreateElement("appender"));

            XmlNodeList appenderRefs = loggerElement.SelectNodes($"appender-ref[@ref='{mNameProperty.Value}']");

            Assert.IsNotNull(appenderRefs);
            Assert.AreEqual(0, appenderRefs.Count);
        }

        [Test]
        public void Save_ShouldRemoveExistingRefs_WhenNotEnabled_AndNameHasChanged()
        {
            XmlElement loggerElement = mXmlDoc.CreateElement("logger");
            mXmlDoc.CreateElementWithAttribute("appender-ref", "ref", mNameProperty.Value).AppendTo(loggerElement);
            mXmlDoc.CreateElementWithAttribute("appender-ref", "ref", mNameProperty.Value).AppendTo(loggerElement);

            mSut.RefsCollection = new ObservableCollection<IAcceptAppenderRef>
            {
                new LoggerModel(loggerElement, false, LoggerDescriptor.Logger)
            };

            //Original name is "appender0"
            mNameProperty.Value = "someOtherName";

            mSut.Save(mXmlDoc, mXmlDoc.CreateElement("appender"));

            XmlNodeList appenderRefs = loggerElement.SelectNodes($"appender-ref[@ref='{mNameProperty.Value}' or @ref='{mNameProperty.OriginalName}']");

            Assert.IsNotNull(appenderRefs);
            Assert.AreEqual(0, appenderRefs.Count);
        }

        [Test]
        public void Save_ShouldRemoveOldIncomingRefs_WhenNameHasChanged_AndRefIsEnabled()
        {
            //Original name is "appender0"
            mNameProperty.Value = "someOtherName";

            //Let's try to add a ref to the asyncAppender (which already exists with the original name)
            IAcceptAppenderRef loggerModel = mSut.RefsCollection.First(r => ((NamedModel)r).Name == "asyncAppender");
            loggerModel.IsEnabled = true;

            mSut.Save(mXmlDoc, mXmlDoc.CreateElement("appender"));

            //Selects all "appender-ref" nodes with the "ref" attribute
            XmlNodeList appenderRefs = loggerModel.Node.SelectNodes("appender-ref[@ref]");

            Assert.IsNotNull(appenderRefs);
            Assert.AreEqual(1, appenderRefs.Count);
            Assert.AreEqual("someOtherName", appenderRefs[0].Attributes["ref"].Value);
        }

        [Test]
        public void Save_ShouldSaveRef_WhenNoneExist()
        {
            XmlElement loggerElement = mXmlDoc.CreateElement("logger");

            mSut.RefsCollection = new ObservableCollection<IAcceptAppenderRef>
            {
                new LoggerModel(loggerElement, true, LoggerDescriptor.Logger)
            };

            mSut.Save(mXmlDoc, mXmlDoc.CreateElement("appender"));

            XmlNodeList appenderRefs = loggerElement.SelectNodes($"appender-ref[@ref='{mNameProperty.Value}']");

            Assert.IsNotNull(appenderRefs);
            Assert.AreEqual(1, appenderRefs.Count);
        }
    }
}
