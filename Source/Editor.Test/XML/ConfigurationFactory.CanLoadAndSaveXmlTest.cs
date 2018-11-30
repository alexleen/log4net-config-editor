// Copyright © 2018 Alex Leendertsen

using System.IO;
using System.Reflection;
using System.Xml;
using SystemInterface.IO;
using SystemInterface.Xml;
using Editor.Interfaces;
using Editor.Utilities;
using Editor.XML;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.XML
{
    [TestFixture]
    public class ConfigurationFactory_CanLoadAndSaveXmlTest
    {
        private const string Filename = "filename";
        private ICanLoadAndSaveXml mSut;
        private IXmlDocument mXmlDoc;
        private bool mFileExists = true;
        private IXmlWriterFactory mXmlWriterFactory;

        [SetUp]
        public void SetUp()
        {
            IConfigurationXml configFactory = new ConfigurationFactory(Substitute.For<IMessageBoxService>()).Create(Filename);

            mSut = (ICanLoadAndSaveXml)configFactory.GetType().GetField("mLoadAndSave", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(configFactory);

            mXmlDoc = Substitute.For<IXmlDocument>();

            IXmlDocumentFactory xmlDocFactory = Substitute.For<IXmlDocumentFactory>();
            xmlDocFactory.Create().Returns(mXmlDoc);

            SetSutField("mXmlDocFactory", xmlDocFactory);

            IFile file = Substitute.For<IFile>();
            file.Exists(Filename).Returns(ci => mFileExists);

            SetSutField("mFile", file);

            mXmlWriterFactory = Substitute.For<IXmlWriterFactory>();
            SetSutField("mXmlWriterFactory", mXmlWriterFactory);

            //Substitute fileStreamFactory to avoid disk access attempts
            IFileStreamFactory fileStreamFactory = Substitute.For<IFileStreamFactory>();
            SetSutField("mFileStreamFactory", fileStreamFactory);
        }

        private void SetSutField(string fieldName, object value)
        {
            mSut.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance).SetValue(mSut, value);
        }

        [Test]
        public void Load_ShouldLoadFromFile_WhenExists()
        {
            IXmlDocument actual = mSut.Load();

            mXmlDoc.Received(1).Load(Filename);

            Assert.AreSame(mXmlDoc, actual);
        }

        [Test]
        public void Load_ShouldNotLoadFromFile_WhenNonexistent()
        {
            mFileExists = false;

            IXmlDocument actual = mSut.Load();

            mXmlDoc.DidNotReceive().Load(Filename);

            Assert.AreSame(mXmlDoc, actual);
        }

        [Test]
        public void Load_ShouldReturnNewXmlDoc_WithLog4NetElement_WhenFileNonexistent()
        {
            mFileExists = false;

            IXmlDocument actual = mSut.Load();

            mXmlDoc.Received(1).CreateElement(Log4NetXmlConstants.Log4Net);

            mXmlDoc.Received(1).AppendChild(mXmlDoc.CreateElement(Log4NetXmlConstants.Log4Net));

            Assert.AreSame(mXmlDoc, actual);
        }

        [Test]
        public void Save_ShouldSaveXml()
        {
            mSut.Load();

            mSut.Save();

            mXmlDoc.Received(1).Save(Arg.Any<XmlWriter>());
        }

        [Test]
        public void Save_ShouldSaveXml_WithCorrectSettings()
        {
            mSut.Load();

            mSut.Save();

            mXmlWriterFactory.Received(1).Create(Arg.Any<FileStream>(), Arg.Is<XmlWriterSettings>(settings => settings.Indent));
        }
    }
}
