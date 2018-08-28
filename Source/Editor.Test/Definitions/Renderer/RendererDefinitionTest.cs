// Copyright © 2018 Alex Leendertsen

using System.Linq;
using Editor.ConfigProperties;
using Editor.Definitions.Renderer;
using NUnit.Framework;

namespace Editor.Test.Definitions.Renderer
{
    [TestFixture]
    public class RendererDefinitionTest
    {
        private RendererDefinition mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new RendererDefinition();
        }

        [Test]
        public void Name_ShouldBeInitializedCorrectly()
        {
            Assert.AreEqual("Renderer", mSut.Name);
        }

        [Test]
        public void Icon_ShouldBeInitializedCorrectly()
        {
            Assert.AreEqual("pack://application:,,,/Editor;component/Images/list-add.png", mSut.Icon);
        }

        [Test]
        public void Initialize_ShouldAddDefaultProperties()
        {
            mSut.Initialize();

            mSut.Properties.Single(p => p.GetType() == typeof(RenderingClass));
            mSut.Properties.Single(p => p.GetType() == typeof(RenderedClass));
        }

        [Test]
        public void Initialize_ShouldAddCorrectNumberOfProperties()
        {
            mSut.Initialize();

            Assert.AreEqual(2, mSut.Properties.Count);
        }
    }
}
