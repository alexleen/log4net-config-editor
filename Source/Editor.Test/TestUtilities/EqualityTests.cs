using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Editor.Test.TestUtilities
{
    [TestFixture]
    public abstract class EqualityTests<TSutType> where TSutType : class, IEquatable<TSutType>
    {
        protected TSutType mSut;

        [SetUp]
        public virtual void SetUp()
        {
            mSut = GetSut();
        }

        protected abstract TSutType GetSut();

        protected abstract TSutType GetOtherEqual();

        protected abstract IEnumerable<TSutType> GetOthersNotEqual();

        protected abstract int ExpectedHashCode { get; }

        #region IEquatable

        [Test]
        public void EquatableEquals_ShouldReturnFalse_WhenNull()
        {
            Assert.IsFalse(mSut.Equals((TSutType)null));
        }

        [Test]
        public void EquatableEquals_ShouldReturnTrue_WhenSameInstance()
        {
            Assert.IsTrue(mSut.Equals(mSut));
        }

        [Test]
        public void EquatableEquals_ShouldReturnTrue_WhenDifferentInstance_ButStructurallyEqual()
        {
            Assert.IsTrue(mSut.Equals(GetOtherEqual()));
        }

        [Test]
        public void EquatableEquals_ShouldReturnFalse_WhenDifferentInstance_ButStructurallyUnequal()
        {
            foreach (TSutType other in GetOthersNotEqual())
            {
                Assert.IsFalse(mSut.Equals(other));
            }
        }

        [Test]
        public void GetHashCode_ShouldReturnCorrectValue()
        {
            Assert.AreEqual(ExpectedHashCode, mSut.GetHashCode());
        }

        #endregion
    }
}
