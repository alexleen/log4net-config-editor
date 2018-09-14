// Copyright © 2018 Alex Leendertsen

using Editor.Interfaces;
using Editor.XML;
using log4net.Core;
using NSubstitute;
using NUnit.Framework;
using System;
using SystemInterface.Xml;
using SystemWrapper.Xml;

namespace Editor.Test.XML
{
    [TestFixture]
    public class ConfigurationXmlTest
    {
        private const string Filename = "filename";

        //private XmlElement mLog4NetElement;
        private IXmlDocument mXmlDoc;
        private IMessageBoxService mMessageBoxService;
        private ConfigurationXml mSut;

        [SetUp]
        public void SetUp()
        {
            mMessageBoxService = Substitute.For<IMessageBoxService>();

            mXmlDoc = new XmlDocumentWrap();

            ICanLoadAndSaveXml loadAndSave = Substitute.For<ICanLoadAndSaveXml>();
            loadAndSave.Load().Returns(mXmlDoc);

            mSut = new ConfigurationXml(Filename,
                                        mMessageBoxService,
                                        loadAndSave);
        }

        [Test]
        public void Ctor_ShouldThrow_WhenFilenameIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ConfigurationXml(null, mMessageBoxService, Substitute.For<ICanLoadAndSaveXml>()));
        }

        [Test]
        public void Ctor_ShouldThrow_WhenMessageBoxServiceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ConfigurationXml(Filename, null, Substitute.For<ICanLoadAndSaveXml>()));
        }

        [Test]
        public void Ctor_ShouldThrow_WhenLoadSaveIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ConfigurationXml(Filename, mMessageBoxService, null));
        }

        [Test]
        public void Threshold_ShouldBeInitializedToAll()
        {
            Assert.AreEqual(Level.All, mSut.Threshold);
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
