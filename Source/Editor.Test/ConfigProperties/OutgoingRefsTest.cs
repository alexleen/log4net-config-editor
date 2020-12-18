// Copyright © 2020 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Models.ConfigChildren;
using Editor.XML;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class OutgoingRefsTest
    {
        [SetUp]
        public void SetUp()
        {
            mXmlDoc = new XmlDocument();
            mXmlDoc.LoadXml("<log4net>\n" +
                            $" <appender name=\"appender0\" type=\"{AppenderDescriptor.Console.TypeNamespace}\">\n" +
                            "    <appender-ref ref=\"appender1\" />\n" +
                            "    <appender-ref ref=\"appender2\" />\n" +
                            "  </appender>\n" +
                            $" <appender name=\"appender1\" type=\"{AppenderDescriptor.Console.TypeNamespace}\">\n" +
                            "    <appender-ref ref=\"appender2\" />\n" +
                            "  </appender>\n" +
                            $" <appender name=\"appender2\" type=\"{AppenderDescriptor.Console.TypeNamespace}\">\n" +
                            "  </appender>\n" +
                            $" <appender name=\"appender3\" type=\"{AppenderDescriptor.Console.TypeNamespace}\">\n" +
                            "  </appender>\n" +
                            $" <appender name=\"asyncAppender\" type=\"{AppenderDescriptor.Async.TypeNamespace}\">\n" +
                            "  </appender>\n" +
                            "  <root>\n" +
                            "  </root>\n" +
                            "</log4net>");

            IElementConfiguration appenderConfiguration = Substitute.For<IElementConfiguration>();
            appenderConfiguration.ConfigXml.Returns(mXmlDoc);
            appenderConfiguration.Log4NetNode.Returns(mXmlDoc.FirstChild);
            appenderConfiguration.OriginalNode.Returns(mXmlDoc.FirstChild["appender"]);
            appenderConfiguration.FindLog4NetNodeChildren("appender").Returns(mXmlDoc.FirstChild.SelectNodes("appender").Cast<XmlNode>());

            mSut = new OutgoingRefs(appenderConfiguration);
        }

        private OutgoingRefs mSut;
        private XmlDocument mXmlDoc;

        [Test]
        public void Ctor_ShouldLoadAvailableRefs()
        {
            //3 appenders + 1 async appender = 4
            Assert.AreEqual(4, mSut.RefsCollection.Count);
            Assert.AreEqual(4, mSut.RefsCollection.Count(r => !r.IsEnabled));
        }

        [Test]
        public void Description_ShouldBeInitializedCorrectly()
        {
            Assert.AreEqual("This element can reference the following appenders:", mSut.Description);
        }

        [Test]
        public void Load_ShouldLoadEnabledRefs()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.OriginalNode.Returns(mXmlDoc.FirstChild["appender"]);

            mSut.Load(config);

            Assert.AreEqual(4, mSut.RefsCollection.Count);
            Assert.AreEqual(2, mSut.RefsCollection.Count(r => r.IsEnabled));
        }

        [Test]
        public void Name_ShouldBeInitializedCorrectly()
        {
            Assert.AreEqual("↑ Refs:", mSut.Name);
        }

        [Test]
        public void Save_ShouldSaveEnabledRefs()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.RefsCollection = new ObservableCollection<AppenderModel>
            {
                new AppenderModel(AppenderDescriptor.Console, null, 0) { IsEnabled = true },
                new AppenderModel(AppenderDescriptor.Console, null, 0) { IsEnabled = true },
                new AppenderModel(AppenderDescriptor.Console, null, 0)
            };

            mSut.Save(config);

            config.Received(2).Save(new Element("appender-ref", new (string attrName, string attrValue)[] { ("ref", null) }));
        }
    }
}
