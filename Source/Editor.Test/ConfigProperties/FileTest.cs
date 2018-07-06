// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class FileTest
    {
        private IMessageBoxService mMessageBoxService;
        private File mSut;

        [SetUp]
        public void SetUp()
        {
            mMessageBoxService = Substitute.For<IMessageBoxService>();
            mSut = new File(new ReadOnlyCollection<IProperty>(new List<IProperty>()), mMessageBoxService);
        }

        [Test]
        public void Open_ShouldShowOpenFilDialog()
        {
            mSut.Open.Execute(null);

            mMessageBoxService.Received(1).ShowOpenFileDialog(out string _);
        }

        [Test]
        public void Open_ShouldSetFilePath()
        {
            mMessageBoxService.ShowOpenFileDialog(out string fileName).Returns(a =>
                {
                    a[0] = "filePath";
                    return true;
                });

            mSut.Open.Execute(null);

            Assert.AreEqual("filePath", mSut.FilePath);
        }

        [Test]
        public void Open_ShouldNotSetFilePath_WhenFileNotChosen()
        {
            mMessageBoxService.ShowOpenFileDialog(out string fileName).Returns(a =>
                {
                    a[0] = null;
                    return false;
                });

            mSut.Open.Execute(null);

            Assert.IsNull(mSut.FilePath);
        }

        [Test]
        public void FilePath_ShouldNotFirePropChange_WhenValueHasNotChanged()
        {
            mSut.FilePath = "filepath";

            bool fired = false;
            mSut.PropertyChanged += (sender, args) => { fired = true; };

            mSut.FilePath = "filepath";

            Assert.IsFalse(fired);
        }

        [Test]
        public void FilePath_ShouldFirePropChange_AndChange_WhenValueHasChanged()
        {
            mSut.FilePath = "filepath";

            bool fired = false;
            mSut.PropertyChanged += (sender, args) => { fired = true; };

            mSut.FilePath = "newfilepath";

            Assert.IsTrue(fired);
            Assert.AreEqual("newfilepath", mSut.FilePath);
        }

        [Test]
        public void Overwrite_ShouldNotFirePropChange_WhenValueHasNotChanged()
        {
            mSut.Overwrite = true;

            bool fired = false;
            mSut.PropertyChanged += (sender, args) => { fired = true; };

            mSut.Overwrite = true;

            Assert.IsFalse(fired);
        }

        [Test]
        public void Overwrite_ShouldFirePropChange_AndChange_WhenValueHasChanged()
        {
            mSut.Overwrite = true;

            bool fired = false;
            mSut.PropertyChanged += (sender, args) => { fired = true; };

            mSut.Overwrite = false;

            Assert.IsTrue(fired);
            Assert.IsFalse(mSut.Overwrite);
        }

        [TestCase(null)]
        [TestCase("<file />")]
        [TestCase("<file value=\"\" />")]
        public void Load_ShouldNotLoadNonExistentFile(string file)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender type=\"log4net.Appender.FileAppender\" name=\"file\">\n" +
                           $"    {file}\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.IsNull(mSut.FilePath);
        }

        [TestCase(null)]
        [TestCase("<appendToFile />")]
        [TestCase("<appendToFile value=\"\" />")]
        public void Load_ShouldNotLoadNonExistentAppendToFile(string appendToFile)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender type=\"log4net.Appender.FileAppender\" name=\"file\">\n" +
                           $"    {appendToFile}\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.IsFalse(mSut.Overwrite);
        }

        [Test]
        public void Load_ShouldLoadFile()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender type=\"log4net.Appender.FileAppender\" name=\"file\">\n" +
                           "    <file value=\"file.log\" />\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual("file.log", mSut.FilePath);
        }

        [TestCase("<appendToFile value=\"true\" />", false)]
        [TestCase("<appendToFile value=\"True\" />", false)]
        [TestCase("<appendToFile value=\"TRUE\" />", false)]
        [TestCase("<appendToFile value=\"false\" />", true)]
        [TestCase("<appendToFile value=\"False\" />", true)]
        [TestCase("<appendToFile value=\"FALSE\" />", true)]
        public void Load_ShouldLoadAppendToFile(string appendToFile, bool expected)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender type=\"log4net.Appender.FileAppender\" name=\"file\">\n" +
                           $"    {appendToFile}\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(expected, mSut.Overwrite);
        }

        [Test]
        public void TryValidate_ShouldSucceed_WhenFilePathIsSpecified()
        {
            mSut.FilePath = "filepath";

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();
            Assert.IsTrue(mSut.TryValidate(messageBoxService));
            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }

        [Test]
        public void TryValidate_ShouldNotSucceed_WhenFilePathIsNotSpecified()
        {
            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();
            Assert.IsFalse(mSut.TryValidate(messageBoxService));
            messageBoxService.Received(1).ShowError("A file must be assigned to this appender.");
        }

        [Test]
        public void Save_ShouldSaveFilePath()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.FilePath = "filepath";
            mSut.Save(xmlDoc, appender);

            XmlNode fileNode = appender.SelectSingleNode("file");

            Assert.IsNotNull(fileNode);
            Assert.AreEqual("filepath", fileNode.Attributes["value"].Value);
        }

        [Test]
        public void Save_ShouldNotSaveAppendTo_WhenOverwriteIsFalse()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Save(xmlDoc, appender);

            //1 for file
            Assert.AreEqual(1, appender.ChildNodes.Count);
        }

        [Test]
        public void Save_ShouldSaveAppendTo_WhenOverwriteIsTrue()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Overwrite = true;
            mSut.Save(xmlDoc, appender);

            XmlNode appendToNode = appender.SelectSingleNode("appendToFile");

            Assert.IsNotNull(appendToNode);
            Assert.AreEqual("false", appendToNode.Attributes["value"].Value);
        }
    }
}
