// Copyright © 2018 Alex Leendertsen

using System.Linq;
using Editor.ConfigProperties;
using Editor.Definitions.Param;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.Definitions.Param
{
    [TestFixture]
    public class ParamDefinitionTest
    {
        private ParamDefinition mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new ParamDefinition(Substitute.For<IElementConfiguration>());
        }

        [Test]
        public void Name_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("Param", mSut.Name);
        }

        [Test]
        public void Icon_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("pack://application:,,,/Editor;component/Images/run-build-configure.png", mSut.Icon);
        }

        [Test]
        public void Initialize_ShouldAddDefaultProperties()
        {
            mSut.Initialize();

            mSut.Properties.Single(p => p.GetType() == typeof(Name));
            mSut.Properties.Single(p => p.GetType() == typeof(Value));
            mSut.Properties.Single(p => p.GetType() == typeof(TypeAttribute));
        }

        [Test]
        public void Initialize_ShouldAddCorrectNumberOfProperties()
        {
            mSut.Initialize();

            Assert.AreEqual(3, mSut.Properties.Count);
        }
    }
}
