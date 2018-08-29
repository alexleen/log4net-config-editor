// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Interfaces;
using Editor.Models;
using Editor.Utilities;
using Editor.Windows;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class ParamsTest
    {
        private IMessageBoxService mMessageBoxService;
        private Params mSut;

        [SetUp]
        public void SetUp()
        {
            mMessageBoxService = Substitute.For<IMessageBoxService>();
            IConfiguration configuration = Substitute.For<IConfiguration>();
            configuration.ConfigXml.Returns(new XmlDocument());

            mSut = new Params(new ReadOnlyCollection<IProperty>(new List<IProperty>()), configuration, mMessageBoxService);
        }

        [Test]
        public void Ctor_ShouldInitializeExistingParams()
        {
            CollectionAssert.IsEmpty(mSut.ExistingParams);
        }

        [Test]
        public void Add_ShouldShowParamElementWindow()
        {
            mSut.Add.Execute(null);

            mMessageBoxService.Received(1).ShowWindow(Arg.Any<ElementWindow>());
        }

        [Test]
        public void Remove_ShouldRemoveParam()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\r\n" +
                           "  <param name=\"someName\" value=\"someValue\" />\r\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            //Test sanity check
            Assert.AreEqual(1, mSut.ExistingParams.Count);

            mSut.ExistingParams[0].Remove.Execute(null);

            CollectionAssert.IsEmpty(mSut.ExistingParams);
        }

        [Test]
        public void Load_ShouldNotLoadNonExistentParams()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\r\n" +
                           "  <whatev />" +
                           "  <para />" +
                           "  <aram />" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            CollectionAssert.IsEmpty(mSut.ExistingParams);
        }

        [Test]
        public void Load_ShouldLoadAllParams()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\r\n" +
                           "  <param name=\"someName\" value=\"someValue\" />\r\n" +
                           "  <param name=\"someName\" type=\"someType\" />\r\n" +
                           "  <param />\r\n" + //We're going to load invalid params ... for now
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(3, mSut.ExistingParams.Count);

            Assert.AreEqual("someName", mSut.ExistingParams[0].Name);
            Assert.AreEqual("someValue", mSut.ExistingParams[0].Value);

            Assert.AreEqual("someName", mSut.ExistingParams[1].Name);
            Assert.AreEqual("someType", mSut.ExistingParams[1].Type);
        }

        [Test]
        public void Save_ShouldSaveAllParams()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\r\n" +
                           "</appender>");

            XmlElement param1 = xmlDoc.CreateElement(Log4NetXmlConstants.Param);
            param1.AppendAttribute(xmlDoc, "name", "someName");
            param1.AppendAttribute(xmlDoc, "value", "someValue");

            mSut.ExistingParams.Add(new ParamModel(param1));

            XmlElement param2 = xmlDoc.CreateElement(Log4NetXmlConstants.Param);
            param2.AppendAttribute(xmlDoc, "name", "someName");
            param2.AppendAttribute(xmlDoc, "type", "someType");

            mSut.ExistingParams.Add(new ParamModel(param2));

            mSut.Save(xmlDoc, xmlDoc.FirstChild);

            Assert.AreEqual(mSut.ExistingParams.Count, xmlDoc.FirstChild.ChildNodes.Count);
            Assert.AreEqual(param1, mSut.ExistingParams[0].Node);
            Assert.AreEqual(param2, mSut.ExistingParams[1].Node);
        }
    }
}
