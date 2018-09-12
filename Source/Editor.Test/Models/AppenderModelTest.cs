using Editor.Descriptors;
using Editor.Models;
using Editor.Models.ConfigChildren;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace Editor.Test.Models
{
    [TestFixture]
    public class AppenderModelTest
    {
        private static IEnumerable<TestCaseData> sAppenders
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
                                                  "</log4net>", expectedType);
                }
            }
        }

        [TestCaseSource(nameof(sAppenders))]
        public void TryCreate_ShouldReturnCorrectType(string xml, Type expectedModelType)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            AppenderModel.TryCreate(xmlDoc.FirstChild.FirstChild, xmlDoc.FirstChild, out AppenderModel model);

            Assert.IsInstanceOf(expectedModelType, model);
        }

        [TestCaseSource(nameof(sAppenders))]
        public void TryCreate_ShouldReturnTrue_ForKnownAppenders(string xml, Type expectedModelType)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            Assert.IsTrue(AppenderModel.TryCreate(xmlDoc.FirstChild.FirstChild, xmlDoc.FirstChild, out AppenderModel _));
        }
    }
}
