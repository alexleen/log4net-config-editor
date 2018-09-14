// Copyright © 2018 Alex Leendertsen

using Editor.Enums;
using Editor.Interfaces;
using Editor.Models;
using Editor.Models.ConfigChildren;
using Editor.Utilities;
using Editor.XML;
using log4net.Core;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
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
        private ICanLoadAndSaveXml mLoadAndSave;
        private ConfigurationXml mSut;

        [SetUp]
        public void SetUp()
        {
            mMessageBoxService = Substitute.For<IMessageBoxService>();

            mXmlDoc = new XmlDocumentWrap();
            mXmlDoc.Load(@"XML\test-config.xml");

            mLoadAndSave = Substitute.For<ICanLoadAndSaveXml>();
            mLoadAndSave.Load().Returns(mXmlDoc);

            mSut = new ConfigurationXml(Filename,
                                        mMessageBoxService,
                                        mLoadAndSave);
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
        public void Debug_ShouldBeInitializedToFalse()
        {
            Assert.IsFalse(mSut.Debug);
        }

        [Test]
        public void Update_ShouldBeInitializedToMerge()
        {
            Assert.AreEqual(Update.Merge, mSut.Update);
        }

        [Test]
        public void Threshold_ShouldBeInitializedToAll()
        {
            Assert.AreEqual(Level.All, mSut.Threshold);
        }

        [Test]
        public void Load_ShouldLoadEverything()
        {
            //Reload is actually the method that loads everything, which should be called within Load
            //Let's just gather all the tests for Reload and run them
            IEnumerable<MethodInfo> reloadMethods = GetType().GetMethods().Where(method => method.Name.StartsWith(nameof(ConfigurationXml.Reload)));

            CollectionAssert.IsNotEmpty(reloadMethods);

            foreach (MethodInfo methodInfo in reloadMethods)
            {
                SetUp();
                methodInfo.Invoke(this, null);
            }
        }

        [Test]
        public void Load_ShouldShowWarning_ForUnknownAppender()
        {
            mXmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><log4net><appender></appender></log4net>");

            mSut.Load();

            mMessageBoxService.Received(1).ShowWarning("At least one unrecognized appender was found in this configuration.");
        }

        [Test]
        public void Reload_ShouldShowError_WhenNoLog4NetElements()
        {
            mXmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><root></root>");

            mSut.Load();
            mMessageBoxService.ClearReceivedCalls();

            //TODO throw IOE if not loaded when Reload() is called?
            mSut.Reload();

            mMessageBoxService.Received(1).ShowError("Could not find log4net configuration.");
        }

        [Test]
        public void Reload_ShouldShowWarning_WhenMoreThanOneLog4NetElement()
        {
            mXmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><root><log4net></log4net><log4net></log4net></root>");

            mSut.Load();
            mMessageBoxService.ClearReceivedCalls();

            //TODO throw IOE if not loaded when Reload() is called?
            mSut.Reload();

            mMessageBoxService.Received(1).ShowWarning("More than one 'log4net' element was found in the specified file. Using the first occurrence.");
        }

        [Test]
        public void Reload_ShouldChooseFirstLog4NetElement()
        {
            mXmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><root><log4net></log4net><log4net></log4net></root>");

            mSut.Load();
            mMessageBoxService.ClearReceivedCalls();

            //TODO throw IOE if not loaded when Reload() is called?
            mSut.Reload();

            Assert.AreSame(mXmlDoc.DocumentElement.FirstChild, mSut.Log4NetNode);
        }

        [Test]
        public void Reload_ShouldLoadAppenders()
        {
            mSut.Load();
            mSut.Reload();

            Assert.AreEqual(2, mSut.Children.OfType<AppenderModel>().Count());
        }

        [Test]
        public void Reload_ShouldLoadRenderers()
        {
            mSut.Load();
            mSut.Reload();

            Assert.AreEqual(1, mSut.Children.OfType<RendererModel>().Count());
        }

        [Test]
        public void Reload_ShouldLoadParams()
        {
            mSut.Load();
            mSut.Reload();

            Assert.AreEqual(1, mSut.Children.OfType<ParamModel>().Count());
        }

        [Test]
        public void Reload_ShouldLoadLoggers()
        {
            mSut.Load();
            mSut.Reload();

            Assert.AreEqual(1, mSut.Children.OfType<LoggerModel>().Count());
            Assert.AreEqual(1, mSut.Children.OfType<RootLoggerModel>().Count());
        }

        [Test]
        public void Reload_ShouldLoadAppenderRefLocations()
        {
            mSut.Load();
            mSut.Reload();

            Assert.AreEqual(2, mSut.Children.OfType<IAcceptAppenderRef>().Count());
        }

        [Test]
        public void Reload_ShouldLoadAttributes()
        {
            mSut.Load();
            mSut.Reload();

            Assert.IsTrue(mSut.Debug);
            Assert.AreEqual(Update.Overwrite, mSut.Update);
            Assert.AreEqual(Level.Error, mSut.Threshold);
        }

        [Test]
        public void Save_ShouldSaveAttributes()
        {
            mSut.Load();

            //Load will set these all to non-defaults
            //Let's set them back to default to make sure the attributes are removed
            mSut.Debug = false;
            mSut.Update = Update.Merge;
            mSut.Threshold = Level.All;

            mSut.Save();

            XmlNode log4netNode = mXmlDoc.DocumentElement.FirstChild;

            Assert.IsNull(log4netNode.Attributes[Log4NetXmlConstants.Debug]);
            Assert.IsNull(log4netNode.Attributes[Log4NetXmlConstants.Update]);
            Assert.IsNull(log4netNode.Attributes[Log4NetXmlConstants.Threshold]);
        }

        [Test]
        public void Save_ShouldSave()
        {
            mSut.Load();

            mSut.Save();

            mLoadAndSave.Received(1).Save();
        }
    }
}
