// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Editor.Test.TestUtilities
{
    [TestFixture]
    public abstract class EqualityTests<TSutType> where TSutType : class, IEquatable<TSutType>
    {
        protected TSutType Sut;

        [SetUp]
        public virtual void SetUp()
        {
            Sut = GetSut();
        }

        protected abstract TSutType GetSut();

        protected abstract TSutType GetOtherEqual();

        protected abstract IEnumerable<TSutType> GetOthersNotEqual();

        protected abstract int ExpectedHashCode { get; }

        #region IEquatable

        [Test]
        public void EquatableEquals_ShouldReturnFalse_WhenNull()
        {
            Assert.IsFalse(Sut.Equals(null));
        }

        [Test]
        public void EquatableEquals_ShouldReturnTrue_WhenSameInstance()
        {
            Assert.IsTrue(Sut.Equals(Sut));
        }

        [Test]
        public void EquatableEquals_ShouldReturnTrue_WhenDifferentInstance_ButStructurallyEqual()
        {
            Assert.IsTrue(Sut.Equals(GetOtherEqual()));
        }

        [Test]
        public void EquatableEquals_ShouldReturnFalse_WhenDifferentInstance_ButStructurallyUnequal()
        {
            foreach (TSutType other in GetOthersNotEqual())
            {
                Assert.IsFalse(Sut.Equals(other));
            }
        }

        #endregion

        #region object

        [Test]
        public void ObjectEquals_ShouldReturnFalse_WhenNull()
        {
            Assert.IsFalse(Sut.Equals((object)null));
        }

        [Test]
        public void ObjectEquals_ShouldReturnTrue_WhenSameInstance()
        {
            Assert.IsTrue(Sut.Equals((object)Sut));
        }

        [Test]
        public void ObjectEquals_ShouldReturnTrue_WhenDifferentInstance_ButStructurallyEqual()
        {
            Assert.IsTrue(Sut.Equals((object)GetOtherEqual()));
        }

        [Test]
        public void ObjectEquals_ShouldReturnFalse_WhenDifferentInstance_ButStructurallyUnequal()
        {
            foreach (TSutType other in GetOthersNotEqual())
            {
                Assert.IsFalse(Sut.Equals((object)other));
            }
        }

        #endregion

        [Test]
        public void GetHashCode_ShouldReturnCorrectValue()
        {
            Assert.AreEqual(ExpectedHashCode, Sut.GetHashCode());
        }
    }
}
