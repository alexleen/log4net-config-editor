// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors;
using Editor.Models;
using NUnit.Framework;
using System.Xml;

namespace Editor.Test.Models
{
    [TestFixture]
    public class RendererModelTest
    {
        private XmlElement mRenderer;
        private RendererModel mSut;

        [SetUp]
        public void SetUp()
        {
            XmlDocument xmlDoc = new XmlDocument();
            mRenderer = xmlDoc.CreateElement(RendererDescriptor.Renderer.ElementName);

            mSut = new RendererModel(mRenderer);
        }

        [Test]
        public void Node_ShouldBeCorrect()
        {
            Assert.AreEqual(mRenderer, mSut.Node);
        }
    }
}
