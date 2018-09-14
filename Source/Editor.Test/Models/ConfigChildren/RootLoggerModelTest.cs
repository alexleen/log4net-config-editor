// Copyright © 2018 Alex Leendertsen

using System.Xml;
using Editor.Descriptors;
using Editor.Models.ConfigChildren;
using NUnit.Framework;

namespace Editor.Test.Models.ConfigChildren
{
    [TestFixture]
    public class RootLoggerModelTest
    {
        [Test]
        public void Name_ShouldReturnNodeName()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<log4net>" +
                           "<root name=\"whatev\">" +
                           "</root>" +
                           "</log4net>");

            RootLoggerModel sut = new RootLoggerModel(xmlDoc.FirstChild.FirstChild, true, LoggerDescriptor.Root);

            Assert.AreEqual("root", sut.Name);
        }
    }
}
