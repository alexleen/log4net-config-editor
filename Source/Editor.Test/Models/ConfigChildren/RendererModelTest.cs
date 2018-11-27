// Copyright © 2018 Alex Leendertsen

using System.Xml;
using Editor.Descriptors;
using Editor.Models.ConfigChildren;
using NUnit.Framework;

namespace Editor.Test.Models.ConfigChildren
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
