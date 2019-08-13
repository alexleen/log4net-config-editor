// Copyright © 2019 Alex Leendertsen

using System;
using System.IO;
using System.Linq;
using Editor.Descriptors;
using Editor.Enums;
using Editor.Interfaces;
using Editor.Models.ConfigChildren;
using Editor.XML;
using log4net.Core;
using NSubstitute;
using NUnit.Framework;
using SystemInterface.Xml;
using SystemWrapper.Xml;

namespace Editor.Test.XML
{
    [TestFixture]
    public class SaveIndicationTest
    {
        private ICanLoadAndSaveXml mLoadAndSave;
        private SaveIndication mSut;

        [SetUp]
        public void SetUp()
        {
            mLoadAndSave = Substitute.For<ICanLoadAndSaveXml>();
            mLoadAndSave.Load().Returns(ci =>
                {
                    IXmlDocument xmlDoc = new XmlDocumentWrap();
                    xmlDoc.Load(GetTestConfigXml());
                    return xmlDoc;
                });

            mSut = new SaveIndication(Substitute.For<IToastService>(), mLoadAndSave);
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

        [Test]
        public void SaveAsync_ShouldUpdateSaveState_ToSaving_WhileSaving()
        {
            mSut.Load();

            bool changedToSaving = false;

            mSut.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(SaveIndication.SaveState) && mSut.SaveState == SaveState.Saving)
                    {
                        changedToSaving = true;
                    }
                };

            mSut.SaveAsync();

            Assert.IsTrue(changedToSaving);

            //Should go back to 'Saved' (because it finished the save)
            Assert.AreEqual(SaveState.Saved, mSut.SaveState);
        }

        [Test]
        public void RemoveChild_ShouldUpdateSaveState()
        {
            mSut.Load();

            mSut.RemoveChild(mSut.Children.First());

            Assert.AreEqual(SaveState.Changed, mSut.SaveState);
        }

        [Test]
        public void ChangingDebug_ShouldChangeSaveState()
        {
            mSut.Load();

            const bool debug = false;

            mSut.Debug = debug;

            Assert.AreEqual(SaveState.Changed, mSut.SaveState);

            Assert.AreEqual(debug, mSut.Debug);
        }

        [Test]
        public void ChangingUpdate_ShouldChangeSaveState()
        {
            mSut.Load();

            const Update update = Update.Merge;

            mSut.Update = update;

            Assert.AreEqual(SaveState.Changed, mSut.SaveState);

            Assert.AreEqual(update, mSut.Update);
        }

        [Test]
        public void ChangingThreshold_ShouldChangeSaveState()
        {
            mSut.Load();

            Level threshold = Level.Off;

            mSut.Threshold = threshold;

            Assert.AreEqual(SaveState.Changed, mSut.SaveState);

            Assert.AreEqual(threshold, mSut.Threshold);
        }
    }
}
