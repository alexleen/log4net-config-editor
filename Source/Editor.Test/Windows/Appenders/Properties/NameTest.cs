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
    public class NameTest
    {
        private Name mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new Name(new ObservableCollection<IProperty>(), null);
        }

        [Test]
        public void IsFocused_ShouldBeTrue()
        {
            Assert.IsTrue(mSut.IsFocused);
        }

        [Test]
        public void Load_ShouldLoadCorrectName()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender name=\"ColoredConsoleAppender\" type=\"log4net.Appender.ColoredConsoleAppender\">\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual("ColoredConsoleAppender", mSut.Value);
        }

        [TestCase("name=\"\"")]
        [TestCase("")]
        public void Load_ShouldNotLoadName(string name)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml($"<appender {name} type=\"log4net.Appender.ColoredConsoleAppender\">\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.IsNull(mSut.Value);
        }

        [TestCase("")]
        [TestCase(null)]
        public void TryValidate_ShouldShowUnassignedMessageBox_WhenValueIsNullOrEmpty_AndReturnFalse(string value)
        {
            mSut.Value = value;

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsFalse(mSut.TryValidate(messageBoxService));
            messageBoxService.Received(1).ShowError("A name must be assigned to this appender.");
        }

        [Test]
        public void TryValidate_ShouldNotShowUnassignedMessageBox_WhenValueIsNotNullOrEmpty_AndReturnTrue()
        {
            mSut.Value = "name";

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsTrue(mSut.TryValidate(messageBoxService));
            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }

        [Test]
        public void TryValidate_ShouldNotShowCollisionMessageBox_WhenNoAppenders()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void TryValidate_ShouldNotShowCollisionMessageBox_WhenNoAppenderNamesCollide()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void TryValidate_ShouldNotShowCollisionMessageBox_WhenAppenderNameCollides()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void Save_ShouldSaveNameToAttribute()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            const string nameValue = "name";
            mSut.Value = nameValue;

            mSut.Save(xmlDoc, appender);

            Assert.AreEqual(nameValue, appender.Attributes["name"].Value);
        }
    }
}
