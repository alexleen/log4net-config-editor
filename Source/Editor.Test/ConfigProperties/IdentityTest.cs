// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Editor.ConfigProperties;
using Editor.Interfaces;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class IdentityTest
    {
        private Identity mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new Identity(new ReadOnlyCollection<IProperty>(new List<IProperty>()));
        }

        [Test]
        public void Name_ShouldBeCorrect()
        {
            Assert.AreEqual("Identity:", mSut.Name);
        }
    }
}
