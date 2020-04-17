// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class StringMatchTest
    {
        private bool mValidateCalled;
        private StringMatch mSut;

        [SetUp]
        public void SetUp()
        {
            mValidateCalled = false;
            mSut = new StringMatch(Validate);
        }

        private bool Validate()
        {
            mValidateCalled = true;
            return false;
        }

        [Test]
        public void TryValidate_ShouldCallValidate()
        {
            //Test sanity check
            Assert.IsFalse(mValidateCalled);

            mSut.TryValidate(Substitute.For<IMessageBoxService>());

            Assert.IsTrue(mValidateCalled);
        }
    }
}
