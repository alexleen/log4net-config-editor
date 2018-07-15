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
    public class RenderingClassTest
    {
        private RenderingClass mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new RenderingClass(new ReadOnlyCollection<IProperty>(new List<IProperty>()));
        }

        [Test]
        public void Name_ShouldBeInitializedCorrectly()
        {
            Assert.AreEqual("Rendering Class:", mSut.Name);
        }

        [Test]
        public void IsFocused_ShouldBeTrue()
        {
            Assert.IsTrue(mSut.IsFocused);
        }

        [Test]
        public void Load_ShouldLoadCorrectRenderingClass()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<renderer renderingClass=\"ColoredConsoleAppender\">\r\n" +
                           "</renderer>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual("ColoredConsoleAppender", mSut.Value);
        }

        [TestCase("renderingClass=\"\"")]
        [TestCase("")]
        public void Load_ShouldNotLoadRenderingClass(string renderingClass)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml($"<renderer {renderingClass}>\r\n" +
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
            messageBoxService.Received(1).ShowError("A valid rendering class must be assigned.");
        }

        [Test]
        public void TryValidate_ShouldNotShowUnassignedMessageBox_WhenValueIsNotNullOrEmpty_AndReturnTrue()
        {
            mSut.Value = "class";

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();

            Assert.IsTrue(mSut.TryValidate(messageBoxService));
            messageBoxService.DidNotReceive().ShowError(Arg.Any<string>());
        }

        [Test]
        public void Save_ShouldSaveRenderingClassToAttribute()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("renderer");

            const string renderingClass = "class";
            mSut.Value = renderingClass;

            mSut.Save(xmlDoc, appender);

            Assert.AreEqual(renderingClass, appender.Attributes["renderingClass"].Value);
        }
    }
}
