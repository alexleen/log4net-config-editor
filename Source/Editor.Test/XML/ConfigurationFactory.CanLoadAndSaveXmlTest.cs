// Copyright © 2018 Alex Leendertsen

using Editor.Interfaces;
using Editor.XML;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.XML
{
    [TestFixture]
    public class ConfigurationFactory_CanLoadAndSaveXmlTest
    {
        private IConfigurationXml mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new ConfigurationFactory(Substitute.For<IMessageBoxService>()).Create("filename");
        }
    }
}
