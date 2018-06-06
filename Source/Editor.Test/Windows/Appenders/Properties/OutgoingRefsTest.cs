// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using Editor.Models;
using Editor.Windows.Appenders.Properties;
using Editor.Windows.PropertyCommon;
using NUnit.Framework;

namespace Editor.Test.Windows.Appenders.Properties
{
    [TestFixture]
    public class OutgoingRefsTest
    {
        private OutgoingRefs mSut;
        private XmlDocument mXmlDoc;

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
                            "  </appender>\n" +
                            "  <root>\n" +
                            "  </root>\n" +
                            "</log4net>");

            mSut = new OutgoingRefs(new ObservableCollection<IProperty>(), mXmlDoc.FirstChild);
        }

        [Test]
        public void Load_ShouldLoadEnabledRefs()
        {
            mSut.Load(mXmlDoc.FirstChild["appender"]);

            Assert.AreEqual(2, mSut.RefsCollection.Count(r => r.IsEnabled));
        }

        [Test]
        public void Load_ShouldLoadAvailableRefs()
        {
            mSut.Load(mXmlDoc.FirstChild["appender"]);

            //3 appenders + 1 async appender + root = 5
            Assert.AreEqual(5, mSut.RefsCollection.Count);
        }

        [Test]
        public void Save_ShouldSaveEnabledRefs()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.RefsCollection = new ObservableCollection<LoggerModel>
            {
                new LoggerModel("name0", appender, true),
                new LoggerModel("name1", appender, true),
                new LoggerModel("name2", appender, false)
            };

            mSut.Save(xmlDoc, appender);

            Assert.AreEqual(2, appender.ChildNodes);
        }
    }
}
