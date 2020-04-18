// Copyright © 2020 Alex Leendertsen

using System.Collections.Generic;
using System.Xml;
using Editor.ConfigProperties;
using Editor.HistoryManager;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class FileTest
    {
        private IMessageBoxService mMessageBoxService;
        private IEnumerable<string> mHistoricalFiles;
        private IHistoryManager mHistoryManager;
        private File mSut;

        [SetUp]
        public void SetUp()
        {
            mMessageBoxService = Substitute.For<IMessageBoxService>();

            mHistoryManager = Substitute.For<IHistoryManager>();
            mHistoricalFiles = new[] { "file1", "file2" };
            mHistoryManager.Get().Returns(mHistoricalFiles);

            IHistoryManagerFactory historyManagerFactory = Substitute.For<IHistoryManagerFactory>();
            historyManagerFactory.CreateFilePathHistoryManager().Returns(mHistoryManager);

            mSut = new File(mMessageBoxService, historyManagerFactory);
        }

        [TestCase("", false)]
        [TestCase("whatev", false)]
        [TestCase("log4net.Util.PatternString", true)]
        public void Load_ShouldLoadPatternStringCorrectly(string value, bool expected)
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("type", out _, "file").Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns(value);
                    ci[1] = result;
                    return true;
                });

            mSut.Load(config);

            Assert.AreEqual(expected, mSut.PatternString);
        }

        [TestCase("true", false)]
        [TestCase("True", false)]
        [TestCase("TRUE", false)]
        [TestCase("false", true)]
        [TestCase("False", true)]
        [TestCase("FALSE", true)]
        public void Load_ShouldLoadAppendToFile(string value, bool expected)
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("value", out _, "appendToFile").Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns(value);
                    ci[1] = result;
                    return true;
                });

            mSut.Load(config);

            Assert.AreEqual(expected, mSut.Overwrite);
        }

        [Test]
        public void Load_ShouldLoadFile()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("value", out _, "file").Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns("file.log");
                    ci[1] = result;
                    return true;
                });

            mSut.Load(config);

            Assert.AreEqual("file.log", mSut.FilePath);
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
        public void FilePath_ShouldNotFirePropChange_WhenValueHasNotChanged()
        {
            mSut.FilePath = "filepath";

            bool fired = false;
            mSut.PropertyChanged += (sender, args) => { fired = true; };

            mSut.FilePath = "filepath";

            Assert.IsFalse(fired);
        }

        [Test]
        public void Open_ShouldNotSetFilePath_WhenFileNotChosen()
        {
            mMessageBoxService.ShowOpenFileDialog(out string _).Returns(a =>
                {
                    a[0] = null;
                    return false;
                });

            mSut.Open.Execute(null);

            Assert.IsNull(mSut.FilePath);
        }

        [Test]
        public void Open_ShouldSetFilePath()
        {
            mMessageBoxService.ShowOpenFileDialog(out string _).Returns(a =>
                {
                    a[0] = "filePath";
                    return true;
                });

            mSut.Open.Execute(null);

            Assert.AreEqual("filePath", mSut.FilePath);
        }

        [Test]
        public void Open_ShouldShowOpenFilDialog()
        {
            mSut.Open.Execute(null);

            mMessageBoxService.Received(1).ShowOpenFileDialog(out string _);
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
        public void PatternString_ShouldFirePropChange_AndChange_WhenValueHasChanged()
        {
            mSut.PatternString = true;

            bool fired = false;
            mSut.PropertyChanged += (sender, args) => { fired = true; };

            mSut.PatternString = false;

            Assert.IsTrue(fired);
            Assert.IsFalse(mSut.PatternString);
        }

        [Test]
        public void PatternString_ShouldNotFirePropChange_WhenValueHasNotChanged()
        {
            mSut.PatternString = true;

            bool fired = false;
            mSut.PropertyChanged += (sender, args) => { fired = true; };

            mSut.PatternString = true;

            Assert.IsFalse(fired);
        }

        [Test]
        public void Properties_ShouldBeInitializedCorrectly()
        {
            Assert.AreSame(mHistoricalFiles, mSut.HistoricalFiles);
            Assert.IsNull(mSut.FilePath);
            Assert.IsFalse(mSut.PatternString);
            Assert.IsFalse(mSut.Overwrite);
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
        public void Save_ShouldNotSavePatternString_WhenPatternStringIsFalse()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.PatternString = false;
            mSut.Save(xmlDoc, appender);

            XmlNode fileNode = appender.SelectSingleNode("file");

            Assert.IsNull(fileNode?.Attributes?["type"]);
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
            Assert.AreEqual("false", appendToNode.Attributes?["value"].Value);
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
            Assert.AreEqual("filepath", fileNode.Attributes?["value"].Value);
        }

        [Test]
        public void Save_ShouldSavePatternString_WhenPatternStringIsTrue()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.PatternString = true;
            mSut.Save(xmlDoc, appender);

            XmlNode fileNode = appender.SelectSingleNode("file");

            Assert.AreEqual("log4net.Util.PatternString", fileNode?.Attributes?["type"].Value);
        }

        [Test]
        public void Save_ShouldSaveToHistoricalFilePaths()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.FilePath = "file3";

            mSut.Save(xmlDoc, appender);

            mHistoryManager.Received(1).Save(mSut.FilePath);
        }

        [Test]
        public void TryValidate_ShouldNotSucceed_WhenFilePathIsNotSpecified()
        {
            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();
            Assert.IsFalse(mSut.TryValidate(messageBoxService));
            messageBoxService.Received(1).ShowError("A file must be assigned to this appender.");
        }

        [Test]
        public void TryValidate_ShouldSucceed_WhenFilePathIsSpecified()
        {
            mSut.FilePath = "filepath";

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();
            Assert.IsTrue(mSut.TryValidate(messageBoxService));
            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }
    }
}
