using Editor.Interfaces;
using Editor.Utilities;
using Editor.XML;
using log4net.Core;
using NSubstitute;
using NUnit.Framework;
using System.Xml;
using SystemInterface.IO;
using SystemInterface.Xml;

namespace Editor.Test.XML
{
    [TestFixture]
    public class ConfigurationXmlTest
    {
        private const string Filename = "filename";
        private XmlElement mLog4NetElement;
        private IXmlDocument mXmlDoc;
        private IMessageBoxService mMessageBoxService;
        private IXmlDocumentFactory mXmlDocFactory;
        private ConfigurationXml mSut;

        [SetUp]
        public void SetUp()
        {
            XmlDocument actualXmlDoc = new XmlDocument();
            mLog4NetElement = actualXmlDoc.CreateElement(Log4NetXmlConstants.Log4Net);

            mXmlDoc = Substitute.For<IXmlDocument>();
            mXmlDoc.CreateElement(Log4NetXmlConstants.Log4Net).Returns(mLog4NetElement);

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
        public void CreateNew_ShouldCallCreate_OnXmlDocumentFactory()
        {
            mSut.CreateNew();

            mXmlDocFactory.Received(1).Create();
        }

        [Test]
        public void CreateNew_ShouldCreate_AndAssignLog4NetNode()
        {
            mSut.CreateNew();

            Assert.AreSame(mLog4NetElement, mSut.Log4NetNode);
        }

        [Test]
        public void CreateNew_ShouldAppendLog4NetNodeToXmlDoc()
        {
            mSut.CreateNew();

            mXmlDoc.Received(1).AppendChild(mLog4NetElement);
        }

        [Test]
        public void CreateNew_ShouldSave()
        {
            mSut.CreateNew();
            //TODO assert root attrs are saved(?)
            mXmlDoc.Received(1).Save(Arg.Any<XmlWriter>());
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

            //1 form Load, 1 from Reload
            mMessageBoxService.Received(2).ShowError("Could not find log4net configuration.");
        }
    }
}
