// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using Editor.Models;
using Editor.Test.TestUtilities;
using log4net.Core;
using NUnit.Framework;

namespace Editor.Test.Models
{
    [TestFixture]
    public class FixModelTest : EqualityTests<FixModel>
    {
        private const FixFlags Flag = FixFlags.None;
        private const bool PerformanceImpact = false;
        private const string Description = "description";

        protected override FixModel GetOtherEqual()
        {
            return new FixModel(Flag, PerformanceImpact, Description);
        }

        protected override IEnumerable<FixModel> GetOthersNotEqual()
        {
            yield return new FixModel(FixFlags.Partial, PerformanceImpact, Description);
            yield return new FixModel(Flag, true, Description);
            yield return new FixModel(Flag, PerformanceImpact, "other description");
            yield return new FixModel(FixFlags.Partial, true, "other description");
        }

        protected override FixModel GetSut()
        {
            return new FixModel(Flag, PerformanceImpact, Description);
        }

        protected override int ExpectedHashCode
        {
            get
            {
                int hashCode = (int)Flag;
                hashCode = (hashCode * 397) ^ PerformanceImpact.GetHashCode();
                hashCode = (hashCode * 397) ^ Description.GetHashCode();
                return hashCode;
            }
        }

        [Test]
        public void Enabled_ShouldFireOnPropertyChanged()
        {
            bool fired = false;
            Sut.PropertyChanged += (sender, args) => { fired = true; };

            Sut.Enabled = true;

            Assert.IsTrue(fired);
        }

        [Test]
        public void Enabled_ShouldSet_WhenNoPropertyChangedSubscription()
        {
            Sut.Enabled = true;

            Assert.IsTrue(Sut.Enabled);
        }
    }
}
