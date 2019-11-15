// Copyright © 2019 Alex Leendertsen

using System;
using System.Xml;
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
            string xml = "<log4net>" +
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
            Assert.IsTrue(ModelFactory.TryCreate(mXmlDoc.FirstChild.ChildNodes[0], mXmlDoc.FirstChild, out ModelBase model));
            Assert.IsInstanceOf<RootLoggerModel>(model);
        }

        [Test]
        public void TryCreate_ShouldReturnLoggerModel()
        {
            Assert.IsTrue(ModelFactory.TryCreate(mXmlDoc.FirstChild.ChildNodes[1], mXmlDoc.FirstChild, out ModelBase model));
            Assert.IsInstanceOf<LoggerModel>(model);
        }

        [Test]
        public void TryCreate_ShouldReturnAppenderModel()
        {
            Assert.IsTrue(ModelFactory.TryCreate(mXmlDoc.FirstChild.ChildNodes[2], mXmlDoc.FirstChild, out ModelBase model));
            Assert.IsInstanceOf<AppenderModel>(model);
        }

        [Test]
        public void TryCreate_ShouldReturnFalse_WhenUnknownAppender()
        {
            Assert.IsFalse(ModelFactory.TryCreate(mXmlDoc.FirstChild.ChildNodes[6], mXmlDoc.FirstChild, out ModelBase model));
            Assert.IsNull(model);
        }

        [Test]
        public void TryCreate_ShouldReturnRendererModel()
        {
            Assert.IsTrue(ModelFactory.TryCreate(mXmlDoc.FirstChild.ChildNodes[3], mXmlDoc.FirstChild, out ModelBase model));
            Assert.IsInstanceOf<RendererModel>(model);
        }

        [Test]
        public void TryCreate_ShouldReturnParamModel()
        {
            Assert.IsTrue(ModelFactory.TryCreate(mXmlDoc.FirstChild.ChildNodes[4], mXmlDoc.FirstChild, out ModelBase model));
            Assert.IsInstanceOf<ParamModel>(model);
        }

        [Test]
        public void TryCreate_ShouldThrow_WhenUnrecognizedNode()
        {
            Assert.Throws<ArgumentException>(() => ModelFactory.TryCreate(mXmlDoc.FirstChild.ChildNodes[5], mXmlDoc.FirstChild, out ModelBase _));
        }
    }
}
