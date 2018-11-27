// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using Editor.Descriptors;
using Editor.Models.ConfigChildren;
using NUnit.Framework;

namespace Editor.Test.Models.ConfigChildren
{
    [TestFixture]
    public class AppenderModelTest
    {
        [Test]
        public void Ctor_ShouldAssignIncomingReferences()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<log4net>" +
                           "<appender>" +
                           "</appender>" +
                           "</log4net>");

            AppenderModel sut = new AppenderModel(AppenderDescriptor.Async, xmlDoc.FirstChild.FirstChild, 1);

            Assert.AreEqual(1, sut.IncomingReferences);
        }

        [Test]
        public void Ctor_ShouldThrow_WhenNodeNameDoesNotMatchDescriptorElementName()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<log4net>" +
                           "<whatev>" +
                           "</whatev>" +
                           "</log4net>");

            Assert.Throws<ArgumentException>(() => new AppenderModel(AppenderDescriptor.Async, xmlDoc.FirstChild.FirstChild, 0));
        }

        [Test]
        public void Name_ShouldBeNull_WhenNodeIsNull()
        {
            AppenderModel sut = new AppenderModel(AppenderDescriptor.Async, null, 0);

            Assert.IsNull(sut.Name);
        }

        [Test]
        public void Name_ShouldBeNull_WhenNodeNameIsNonExistent()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<log4net>" +
                           "<appender>" +
                           "</appender>" +
                           "</log4net>");

            AppenderModel sut = new AppenderModel(AppenderDescriptor.Async, xmlDoc.FirstChild.FirstChild, 0);

            Assert.IsNull(sut.Name);
        }

        private static IEnumerable<TestCaseData> Appenders
        {
            get
            {
                FieldInfo[] appenders = typeof(AppenderDescriptor).GetFields(BindingFlags.Public | BindingFlags.Static);

                foreach (FieldInfo fieldInfo in appenders)
                {
                    AppenderDescriptor descriptor = (AppenderDescriptor)fieldInfo.GetValue(null);

                    Type expectedType = descriptor == AppenderDescriptor.Async ? typeof(AsyncAppenderModel) : typeof(AppenderModel);

                    yield return new TestCaseData("<log4net>" +
                                                  $"<appender type=\"{descriptor.TypeNamespace}\">" +
                                                  "</appender>" +
                                                  "</log4net>",
                                                  expectedType);
                }
            }
        }

        [TestCaseSource(nameof(Appenders))]
        public void TryCreate_ShouldReturnCorrectType(string xml, Type expectedModelType)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            AppenderModel.TryCreate(xmlDoc.FirstChild.FirstChild, xmlDoc.FirstChild, out AppenderModel model);

            Assert.IsInstanceOf(expectedModelType, model);
        }

        [TestCaseSource(nameof(Appenders))]
        public void TryCreate_ShouldReturnTrue_ForKnownAppenders(string xml, Type expectedModelType)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            Assert.IsTrue(AppenderModel.TryCreate(xmlDoc.FirstChild.FirstChild, xmlDoc.FirstChild, out AppenderModel _));
        }

        [Test]
        public void TryCreate_ShouldReturnNull_ForUnknownAppenders()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<log4net>" +
                           "<appender>" +
                           "</appender>" +
                           "</log4net>");

            AppenderModel.TryCreate(xmlDoc.FirstChild.FirstChild, xmlDoc.FirstChild, out AppenderModel model);

            Assert.IsNull(model);
        }

        [Test]
        public void TryCreate_ShouldReturnFalse_ForUnknownAppenders()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<log4net>" +
                           "<appender>" +
                           "</appender>" +
                           "</log4net>");

            Assert.IsFalse(AppenderModel.TryCreate(xmlDoc.FirstChild.FirstChild, xmlDoc.FirstChild, out AppenderModel _));
        }
    }
}
