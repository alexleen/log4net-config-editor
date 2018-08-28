// Copyright © 2018 Alex Leendertsen

using System.Linq;
using Editor.ConfigProperties;
using Editor.Definitions.Mapping;
using NUnit.Framework;

namespace Editor.Test.Definitions.Mapping
{
    [TestFixture]
    public class MappingDefinitionTest
    {
        private MappingDefinition mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new MappingDefinition();
        }

        [Test]
        public void Name_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("Mapping", mSut.Name);
        }

        [Test]
        public void Icon_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("pack://application:,,,/Editor;component/Images/fill-color.png", mSut.Icon);
        }

        [Test]
        public void Initialize_ShouldAddDefaultProperties()
        {
            mSut.Initialize();

            mSut.Properties.Single(p => p.GetType() == typeof(LevelProperty));
            mSut.Properties.Single(p => p.GetType() == typeof(ForeColor));
            mSut.Properties.Single(p => p.GetType() == typeof(BackColor));
        }

        [Test]
        public void Initialize_ShouldAddCorrectNumberOfProperties()
        {
            mSut.Initialize();

            Assert.AreEqual(3, mSut.Properties.Count);
        }
    }
}
