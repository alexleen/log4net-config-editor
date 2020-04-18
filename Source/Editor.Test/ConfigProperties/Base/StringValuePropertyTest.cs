// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties.Base
{
    [TestFixture]
    public class StringValuePropertyTest
    {
        private StringValueProperty mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new StringValueProperty("name", "element");
        }

        [Test]
        public void Load_ShouldNotSetValue_WhenLoadFails()
        {
            //Sanity check
            Assert.IsNull(mSut.Value);

            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("value", out _, "element").Returns(false);

            mSut.Load(config);

            Assert.IsNull(mSut.Value);
        }
    }
}
