// Copyright © 2018 Alex Leendertsen

using System.Xml;
using SystemInterface.IO;
using SystemInterface.Xml;
using SystemWrapper.Xml;
using Editor.Interfaces;
using Editor.XML;
using log4net.Core;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.XML
{
    [TestFixture]
    public class ConfigurationXmlTest
    {
        private const string Filename = "filename";

        //private XmlElement mLog4NetElement;
        private IXmlDocument mXmlDoc;
        private IMessageBoxService mMessageBoxService;
        private IXmlDocumentFactory mXmlDocFactory;
        private ConfigurationXml mSut;

        [SetUp]
        public void SetUp()
        {
            //XmlDocument actualXmlDoc = new XmlDocument();
            //mLog4NetElement = actualXmlDoc.CreateElement(Log4NetXmlConstants.Log4Net);

            mXmlDoc = new XmlDocumentWrap();
            //mXmlDoc.CreateElement(Log4NetXmlConstants.Log4Net).Returns(mLog4NetElement);

            mXmlDocFactory = Substitute.For<IXmlDocumentFactory>();
            mXmlDocFactory.Create().Returns(mXmlDoc);

            mMessageBoxService = Substitute.For<IMessageBoxService>();
            mSut = new ConfigurationXml(Filename,
                                        mMessageBoxService,
                                        mXmlDocFactory,
                                        Substitute.For<IFileStreamFactory>(),
                                        Substitute.For<IXmlWriterFactory>());
        }

        [Test]
        public void Threshold_ShouldBeInitializedToAll()
        {
            Assert.AreEqual(Level.All, mSut.Threshold);
        }

        [Test]
        public void CreateNew_ShouldCreate_AndAssignLog4NetNode()
        {
            mSut.CreateNew();

            XmlNodeList roots = mXmlDoc.SelectNodes("//log4net");

            Assert.AreEqual(1, roots.Count);
            Assert.AreSame(roots[0], mSut.Log4NetNode);
        }

        [Test]
        public void Load_ShouldCallCreate_OnXmlDocumentFactory()
        {
            mSut.Load();

            mXmlDocFactory.Received(1).Create();
        }

        [Test]
        public void Load_ShouldLoadIntoXmlDoc()
        {
            mSut.Load();

            mXmlDoc.Received(1).Load(Filename);
        }

        [Test]
        public void Reload_ShouldShowError_WhenNoLog4NetElements()
        {
            mSut.Load();
            //TODO throw IOE if not loaded when Reload() is called?
            mSut.Reload();

            //1 from Load + 1 from Reload
            mMessageBoxService.Received(2).ShowError("Could not find log4net configuration.");
        }
    }
}
