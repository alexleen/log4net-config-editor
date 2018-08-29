// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Editor.ConfigProperties;
using Editor.Interfaces;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class LocalPortTest : PortTestBase
    {
        internal override Port GetSut()
        {
            return new LocalPort(new ReadOnlyCollection<IProperty>(new List<IProperty>()));
        }

        [Test]
        public void Name_ShouldBeCorrect()
        {
            Assert.AreEqual("Local Port:", Sut.Name);
        }

        [TestCase(null, null)]
        [TestCase("<localPort />", null)]
        [TestCase("<localPort value=\"\" />", null)]
        [TestCase("<localPort value=\"1234\" />", "1234")]
        public void Load_ShouldLoadCorrectly(string xml, string expected)
        {
            TestLoadWithXml(xml, expected);
        }

        [Test]
        public void Save_ShouldSave()
        {
            TestSave("localPort");
        }
    }
}
