// Copyright © 2019 Alex Leendertsen

using System;
using System.IO;
using System.Linq;
using SystemInterface.Xml;
using SystemWrapper.Xml;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Models.ConfigChildren;
using Editor.XML;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.XML
{
    [TestFixture]
    public class SaveIndicationTest
    {
        private IXmlDocument mXmlDoc;
        private ICanLoadAndSaveXml mLoadAndSave;
        private SaveIndication mSut;

        [SetUp]
        public void SetUp()
        {
            mXmlDoc = new XmlDocumentWrap();
            mXmlDoc.Load(GetTestConfigXml());

            mLoadAndSave = Substitute.For<ICanLoadAndSaveXml>();
            mLoadAndSave.Load().Returns(mXmlDoc);

            mSut = new SaveIndication(Substitute.For<IMessageBoxService>(), mLoadAndSave);
        }

        /// <summary>
        /// Retrieves a path to the test-config.xml file.
        /// </summary>
        /// <returns></returns>
        private static string GetTestConfigXml()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"XML\test-config.xml");
        }

        [Test]
        public void CreateElementConfigurationFor_ShouldCreateElementConfigWithCorrectProperties_WhenModelIsNull()
        {
            mSut.Load();

            IElementConfiguration elementConfiguration = mSut.CreateElementConfigurationFor(null, AppenderDescriptor.Async.ElementName);

            Assert.IsNull(elementConfiguration.OriginalNode);
            Assert.AreEqual(AppenderDescriptor.Async.ElementName, elementConfiguration.NewNode.Name);
            Assert.AreSame(mSut.ConfigXml, elementConfiguration.ConfigXml);
            Assert.AreSame(mSut.Log4NetNode, elementConfiguration.Log4NetNode);
        }

        [Test]
        public void CreateElementConfigurationFor_ShouldCreateElementConfigWithCorrectProperties()
        {
            mSut.Load();

            AsyncAppenderModel originalModel = mSut.Children.OfType<AsyncAppenderModel>().First();
            IElementConfiguration elementConfiguration = mSut.CreateElementConfigurationFor(originalModel, AppenderDescriptor.Async.ElementName);

            Assert.AreSame(originalModel.Node, elementConfiguration.OriginalNode);
            Assert.AreEqual(AppenderDescriptor.Async.ElementName, elementConfiguration.NewNode.Name);
            Assert.AreSame(mSut.ConfigXml, elementConfiguration.ConfigXml);
            Assert.AreSame(mSut.Log4NetNode, elementConfiguration.Log4NetNode);
        }
    }
}
