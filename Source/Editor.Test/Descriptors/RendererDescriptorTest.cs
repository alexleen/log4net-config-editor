// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors;
using NUnit.Framework;

namespace Editor.Test.Descriptors
{
    [TestFixture]
    public class RendererDescriptorTest
    {
        [Test]
        public void Renderer_ShouldHaveCorrectName()
        {
            Assert.AreEqual("Renderer", RendererDescriptor.Renderer.Name);
        }
    }
}
