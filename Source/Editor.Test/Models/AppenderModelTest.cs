using Editor.Models;
using Editor.Models.ConfigChildren;
using NUnit.Framework;
using System;
using System.Xml;

namespace Editor.Test.Models
{
    [TestFixture]
    public class AppenderModelTest
    {
        [TestCase("<log4net>" +
                    "<appender type=\"Log4Net.Async.AsyncForwardingAppender,Log4Net.Async\">" +
                    "</appender>" +
                  "</log4net>", typeof(AsyncAppenderModel))]
        [TestCase("<log4net>" +
                    "<appender type=\"log4net.Appender.ConsoleAppender\">" +
                    "</appender>" +
                  "</log4net>", typeof(AppenderModel))]
        public void TryCreate_ShouldReturnCorrectType(string xml, Type modelType)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            AppenderModel.TryCreate(xmlDoc.FirstChild.FirstChild, xmlDoc.FirstChild, out AppenderModel model);

            Assert.IsInstanceOf(modelType, model);
        }
    }
}
