// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Editor.ConfigProperties;
using Editor.Interfaces;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class RemotePortTest : PortTestBase
    {
        internal override Port GetSut()
        {
            return new RemotePort(new ReadOnlyCollection<IProperty>(new List<IProperty>()));
        }

        [Test]
        public void Name_ShouldBeCorrect()
        {
            Assert.AreEqual("Remote Port:", Sut.Name);
        }

        [TestCase(null, null)]
        [TestCase("<remotePort />", null)]
        [TestCase("<remotePort value=\"\" />", null)]
        [TestCase("<remotePort value=\"1234\" />", "1234")]
        public void Load_ShouldLoadCorrectly(string xml, string expected)
        {
            TestLoadWithXml(xml, expected);
        }

        [Test]
        public void Save_ShouldSave()
        {
            TestSave("remotePort");
        }
    }
}
