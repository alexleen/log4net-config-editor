// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Interfaces;
using log4net.Appender;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class RollingStyleTest
    {
        private RollingStyle mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new RollingStyle(new ReadOnlyCollection<IProperty>(new List<IProperty>()));
        }

        [Test]
        public void Modes_ShouldBeInitializedCorrectly()
        {
            CollectionAssert.AreEqual(Enum.GetValues(typeof(RollingFileAppender.RollingMode)).Cast<RollingFileAppender.RollingMode>(), mSut.Modes);
        }

        [Test]
        public void SelectedMode_ShouldBeInitializedToComposite()
        {
            Assert.AreEqual(RollingFileAppender.RollingMode.Composite, mSut.SelectedMode);
        }

        [Test]
        public void SelectedMode_ShouldNotFirePropChange_WhenValueHasNotChanged()
        {
            bool fired = false;
            mSut.PropertyChanged += (sender, args) => { fired = true; };

            mSut.SelectedMode = mSut.SelectedMode;

            Assert.IsFalse(fired);
        }

        [Test]
        public void SelectedMode_ShouldFirePropChange_AndChange_WhenValueHasChanged()
        {
            bool fired = false;
            mSut.PropertyChanged += (sender, args) => { fired = true; };

            mSut.SelectedMode = RollingFileAppender.RollingMode.Date;

            Assert.IsTrue(fired);
            Assert.AreEqual(RollingFileAppender.RollingMode.Date, mSut.SelectedMode);
        }

        [TestCase(null)]
        [TestCase("<rollingStyle />")]
        [TestCase("<rollingStyle value=\"\" />")]
        [TestCase("<rollingStyle value=\"whatev\" />")]
        public void Load_ShouldNotLoadMode(string mode)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender type=\"log4net.Appender.RollingFileAppender\" name=\"rolling\">\n" +
                           $"    {mode}\n" +
                           "  </appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(RollingFileAppender.RollingMode.Composite, mSut.SelectedMode);
        }

        [Test]
        public void Load_ShouldLoadMode()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender type=\"log4net.Appender.RollingFileAppender\" name=\"rolling\">\n" +
                           "    <rollingStyle value=\"Date\" />\n" +
                           "  </appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(RollingFileAppender.RollingMode.Date, mSut.SelectedMode);
        }

        [Test]
        public void Save_ShouldNotSave_WhenComposite()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Save(xmlDoc, appender);

            CollectionAssert.IsEmpty(appender.ChildNodes);
        }

        [Test]
        public void Save_ShouldSave_WhenNotComposite()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.SelectedMode = RollingFileAppender.RollingMode.Date;
            mSut.Save(xmlDoc, appender);

            XmlNode rollingStyleNode = appender.SelectSingleNode("rollingStyle");

            Assert.IsNotNull(rollingStyleNode);
            Assert.AreEqual("Date", rollingStyleNode.Attributes["value"].Value);
        }
    }
}
