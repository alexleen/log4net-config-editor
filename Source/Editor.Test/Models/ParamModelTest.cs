// Copyright © 2018 Alex Leendertsen

using System.Xml;
using Editor.Models;
using Editor.Utilities;
using NUnit.Framework;

namespace Editor.Test.Models
{
    [TestFixture]
    public class ParamModelTest
    {
        private ParamModel mSut;

        [SetUp]
        public void SetUp()
        {
            XmlDocument xmlDoc = new XmlDocument();

            mSut = new ParamModel(xmlDoc.CreateElement(Log4NetXmlConstants.Param));
        }

        [TestCase("<param value=\"someValue\" />", null)]
        [TestCase("<param name=\"someName\" value=\"someValue\" />", "someName")]
        public void Name_ShouldReturnCorrectValue(string xml, string expectedName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            mSut = new ParamModel(xmlDoc.FirstChild);

            Assert.AreEqual(expectedName, mSut.Name);
        }

        [TestCase("<param name=\"someName\" />", null)]
        [TestCase("<param name=\"someName\" value=\"someValue\" />", "someValue")]
        public void Value_ShouldReturnCorrectValue(string xml, string expectedValue)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            mSut = new ParamModel(xmlDoc.FirstChild);

            Assert.AreEqual(expectedValue, mSut.Value);
        }

        [TestCase("<param name=\"someName\" />", null)]
        [TestCase("<param name=\"someName\" type=\"someType\" />", "someType")]
        public void Type_ShouldReturnCorrectValue(string xml, string expectedType)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            mSut = new ParamModel(xmlDoc.FirstChild);

            Assert.AreEqual(expectedType, mSut.Type);
        }

        [Test]
        public void Edit_ShouldShowFilterWindow()
        {
            XmlDocument xmlDoc = new XmlDocument();

            bool shown = false;
            mSut = new ParamModel(xmlDoc.CreateElement(Log4NetXmlConstants.Param),
                                  model =>
                                      {
                                          if (model == mSut)
                                          {
                                              shown = true;
                                          }
                                      });

            mSut.Edit.Execute(null);

            Assert.IsTrue(shown);
        }

        [Test]
        public void Remove_ShouldInvokeRemove()
        {
            XmlDocument xmlDoc = new XmlDocument();

            bool shown = false;
            mSut = new ParamModel(xmlDoc.CreateElement(Log4NetXmlConstants.Param),
                                  remove: model =>
                                      {
                                          if (model == mSut)
                                          {
                                              shown = true;
                                          }
                                      });

            mSut.Remove.Execute(null);

            Assert.IsTrue(shown);
        }

        [TestCase(nameof(ParamModel.Name))]
        [TestCase(nameof(ParamModel.Type))]
        [TestCase(nameof(ParamModel.Value))]
        public void NodeSet_ShouldFireOnPropertyChanged(string propName)
        {
            bool fired = false;
            mSut.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == propName)
                    {
                        fired = true;
                    }
                };

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<param name=\"someName\" value=\"someValue\" type=\"someType\" />");

            mSut.Node = xmlDoc.FirstChild;

            Assert.IsTrue(fired);
        }
    }
}
