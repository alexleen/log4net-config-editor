// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Xml;
using Editor.Windows;
using Editor.Windows.Appenders.Properties;
using Editor.Windows.PropertyCommon;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.Windows.Appenders.Properties
{
    [TestFixture]
    public class FileTest
    {
        private File mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new File(new ObservableCollection<IProperty>());
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
