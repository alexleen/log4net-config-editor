// Copyright © 2018 Alex Leendertsen

using System.Xml;
using Editor.ConfigProperties;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class RenderedClassTest
    {
        [SetUp]
        public void SetUp()
        {
            mSut = new RenderedClass();
        }

        private RenderedClass mSut;

        [TestCase("renderedClass=\"\"")]
        [TestCase("")]
        public void Load_ShouldNotLoadRenderedClass(string renderedClass)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml($"<renderer {renderedClass}>\r\n" +
                           "</renderer>");

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
            messageBoxService.Received(1).ShowError("A valid rendered class must be assigned.");
        }

        [Test]
        public void Load_ShouldLoadCorrectRenderedClass()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<renderer renderedClass=\"ColoredConsoleAppender\">\r\n" +
                           "</renderer>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual("ColoredConsoleAppender", mSut.Value);
        }

        [Test]
        public void Name_ShouldBeInitializedCorrectly()
        {
            Assert.AreEqual("Rendered Class:", mSut.Name);
        }

        [Test]
        public void Save_ShouldSaveRenderedClassToAttribute()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("renderer");

            const string renderedClass = "class";
            mSut.Value = renderedClass;

            mSut.Save(xmlDoc, appender);

            Assert.AreEqual(renderedClass, appender.Attributes["renderedClass"].Value);
        }

        [Test]
        public void TryValidate_ShouldNotShowUnassignedMessageBox_WhenValueIsNotNullOrEmpty_AndReturnTrue()
        {
            mSut.Value = "class";

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsTrue(mSut.TryValidate(messageBoxService));
            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }
    }
}
