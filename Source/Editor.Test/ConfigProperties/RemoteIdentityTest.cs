// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class RemoteIdentityTest
    {
        private RemoteIdentity mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new RemoteIdentity();
        }

        [Test]
        public void Name_ShouldBeCorrect()
        {
            Assert.AreEqual("Identity:", mSut.Name);
        }

        [Test]
        public void ToolTip_ShouldBeCorrect()
        {
            Assert.AreEqual("Enter remote syslog identity pattern here.", mSut.ToolTip);
        }
    }
}
