// Copyright © 2020 Alex Leendertsen

using System.Xml;
using Editor.Enums;
using Editor.Models;
using Editor.Models.Base;
using Editor.Models.ConfigChildren;
using NUnit.Framework;

namespace Editor.Test.Models
{
    [TestFixture]
    public class ModelFactoryTest
    {
        private XmlDocument mXmlDoc;

        [SetUp]
        public void SetUp()
        {
            const string xml = "<log4net>" +
                               "    <root>" +
                               "    </root>" +
                               "    <logger>" +
                               "    </logger>" +
                               "    <appender type=\"log4net.Appender.ConsoleAppender\">" +
                               "    </appender>" +
                               "    <renderer>" +
                               "    </renderer>" +
                               "    <param>" +
                               "    </param>" +
                               "    <whatev>" +
                               "    </whatev>" +
                               "    <appender type=\"whatev\">" +
                               "    </appender>" +
                               "</log4net>";

            mXmlDoc = new XmlDocument();
            mXmlDoc.LoadXml(xml);
        }

        [Test]
        public void TryCreate_ShouldReturnRootLoggerModel()
        {
            Assert.AreEqual(ModelCreateResult.Success, ModelFactory.TryCreate(mXmlDoc.FirstChild.ChildNodes[0], mXmlDoc.FirstChild, out ModelBase model));
            Assert.IsInstanceOf<RootLoggerModel>(model);
        }

        [Test]
        public void TryCreate_ShouldReturnLoggerModel()
        {
            Assert.AreEqual(ModelCreateResult.Success, ModelFactory.TryCreate(mXmlDoc.FirstChild.ChildNodes[1], mXmlDoc.FirstChild, out ModelBase model));
            Assert.IsInstanceOf<LoggerModel>(model);
        }

        [Test]
        public void TryCreate_ShouldReturnAppenderModel()
        {
            Assert.AreEqual(ModelCreateResult.Success, ModelFactory.TryCreate(mXmlDoc.FirstChild.ChildNodes[2], mXmlDoc.FirstChild, out ModelBase model));
            Assert.IsInstanceOf<AppenderModel>(model);
        }

        [Test]
        public void TryCreate_ShouldReturnUnknownAppender_WhenUnknownAppender()
        {
            Assert.AreEqual(ModelCreateResult.UnknownAppender, ModelFactory.TryCreate(mXmlDoc.FirstChild.ChildNodes[6], mXmlDoc.FirstChild, out ModelBase model));
            Assert.IsNull(model);
        }

        [Test]
        public void TryCreate_ShouldReturnRendererModel()
        {
            Assert.AreEqual(ModelCreateResult.Success, ModelFactory.TryCreate(mXmlDoc.FirstChild.ChildNodes[3], mXmlDoc.FirstChild, out ModelBase model));
            Assert.IsInstanceOf<RendererModel>(model);
        }

        [Test]
        public void TryCreate_ShouldReturnParamModel()
        {
            Assert.AreEqual(ModelCreateResult.Success, ModelFactory.TryCreate(mXmlDoc.FirstChild.ChildNodes[4], mXmlDoc.FirstChild, out ModelBase model));
            Assert.IsInstanceOf<ParamModel>(model);
        }

        [Test]
        public void TryCreate_ShouldReturnUnknownElement_WhenUnrecognizedNode()
        {
            Assert.AreEqual(ModelCreateResult.UnknownElement, ModelFactory.TryCreate(mXmlDoc.FirstChild.ChildNodes[5], mXmlDoc.FirstChild, out ModelBase _));
        }
    }
}
