// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Models.ConfigChildren;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
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
                            $"  <appender name=\"appender0\" type=\"{AppenderDescriptor.Console.TypeNamespace}\">\n" +
                            "    <appender-ref ref=\"appender1\" />\n" +
                            "    <appender-ref ref=\"appender2\" />\n" +
                            "  </appender>\n" +
                            $"  <appender name=\"appender1\" type=\"{AppenderDescriptor.Console.TypeNamespace}\">\n" +
                            "    <appender-ref ref=\"appender2\" />\n" +
                            "  </appender>\n" +
                            $"  <appender name=\"appender2\" type=\"{AppenderDescriptor.Console.TypeNamespace}\">\n" +
                            "  </appender>\n" +
                            $"  <appender name=\"appender3\" type=\"{AppenderDescriptor.Console.TypeNamespace}\">\n" +
                            "  </appender>\n" +
                            $"  <appender name=\"asyncAppender\" type=\"{AppenderDescriptor.Async.TypeNamespace}\">\n" +
                            "  </appender>\n" +
                            "  <root>\n" +
                            "  </root>\n" +
                            "</log4net>");

            IElementConfiguration appenderConfiguration = Substitute.For<IElementConfiguration>();
            appenderConfiguration.ConfigXml.Returns(mXmlDoc);
            appenderConfiguration.Log4NetNode.Returns(mXmlDoc.FirstChild);
            appenderConfiguration.OriginalNode.Returns(mXmlDoc.FirstChild["appender"]);

            mSut = new OutgoingRefs(new ReadOnlyCollection<IProperty>(new List<IProperty>()), appenderConfiguration, Substitute.For<IAppenderFactory>());
        }

        [Test]
        public void Name_ShouldBeInitializedCorrectly()
        {
            Assert.AreEqual("↑ Refs:", mSut.Name);
        }

        [Test]
        public void Description_ShouldBeInitializedCorrectly()
        {
            Assert.AreEqual("This element can reference the following appenders:", mSut.Description);
        }

        [Test]
        public void Ctor_ShouldLoadAvailableRefs()
        {
            //3 appenders + 1 async appender = 4
            Assert.AreEqual(4, mSut.RefsCollection.Count);
            Assert.AreEqual(4, mSut.RefsCollection.Count(r => !r.IsEnabled));
        }

        [Test]
        public void Load_ShouldLoadEnabledRefs()
        {
            mSut.Load(mXmlDoc.FirstChild["appender"]);

            Assert.AreEqual(4, mSut.RefsCollection.Count);
            Assert.AreEqual(2, mSut.RefsCollection.Count(r => r.IsEnabled));
        }

        [Test]
        public void Save_ShouldSaveEnabledRefs()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.RefsCollection = new ObservableCollection<AppenderModel>
            {
                new AppenderModel(AppenderDescriptor.Console, appender, 0) { IsEnabled = true },
                new AppenderModel(AppenderDescriptor.Console, appender, 0) { IsEnabled = true },
                new AppenderModel(AppenderDescriptor.Console, appender, 0)
            };

            mSut.Save(xmlDoc, appender);

            Assert.AreEqual(2, appender.ChildNodes.Count);
        }
    }
}
