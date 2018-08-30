// Copyright © 2018 Alex Leendertsen

using System;
using System.Xml;
using Editor.Interfaces;
using Editor.SaveStrategies;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.SaveStrategies
{
    [TestFixture]
    public class AppendReplaceSaveStrategyTest
    {
        [Test]
        public void Ctor_ShouldThrow_WhenConfigurationIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AppendReplaceSaveStrategy(null));
        }

        [Test]
        public void Execute_ShouldAppend_WhenOriginalNodeIsNull()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement log4NetElement = xmlDoc.CreateElement("log4net");
            XmlElement newElement = xmlDoc.CreateElement("appender");

            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Log4NetNode.Returns(log4NetElement);
            config.NewNode.Returns(newElement);

            ISaveStrategy strategy = new AppendReplaceSaveStrategy(config);
            strategy.Execute();

            Assert.AreEqual(newElement, log4NetElement.FirstChild);
        }

        [Test]
        public void Execute_ShouldReplace_WhenOriginalNodeIsNotNull()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement log4NetElement = xmlDoc.CreateElement("log4net");
            XmlElement origElement = xmlDoc.CreateElement("origAppender");
            log4NetElement.AppendChild(origElement);
            XmlElement newElement = xmlDoc.CreateElement("newAppender");

            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Log4NetNode.Returns(log4NetElement);
            config.OriginalNode.Returns(origElement);
            config.NewNode.Returns(newElement);

            ISaveStrategy strategy = new AppendReplaceSaveStrategy(config);
            strategy.Execute();

            Assert.AreEqual(1, log4NetElement.ChildNodes.Count);
            Assert.AreEqual(newElement, log4NetElement.FirstChild);
        }
    }
}
