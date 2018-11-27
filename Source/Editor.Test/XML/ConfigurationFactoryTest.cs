// Copyright © 2018 Alex Leendertsen

using Editor.Interfaces;
using Editor.XML;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.XML
{
    [TestFixture]
    public class ConfigurationFactoryTest
    {
        private ConfigurationFactory mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new ConfigurationFactory(Substitute.For<IMessageBoxService>());
        }

        [Test]
        public void Create_ShouldReturnConfigurationXml()
        {
            Assert.IsInstanceOf<ConfigurationXml>(mSut.Create("filename"));
        }
    }
}
