using Editor.Models;
using Editor.Test.TestUtilities;
using log4net.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Editor.Test.Models
{
    [TestFixture]
    public class MappingModelTest : EqualityTests<MappingModel>
    {
        private static readonly Level LevelConst = Level.All;
        private const ConsoleColor ForeColor = ConsoleColor.Black;
        private const ConsoleColor BackColor = ConsoleColor.Blue;

        [Test]
        public void ToString_ShouldReturnCorrectString()
        {
            Assert.AreEqual($"{LevelConst} | {ForeColor} | {BackColor}", mSut.ToString());
        }

        protected override MappingModel GetSut()
        {
            return new MappingModel(null, LevelConst, ForeColor, BackColor, null, null);
        }

        protected override MappingModel GetOtherEqual()
        {
            return new MappingModel(null, LevelConst, ForeColor, BackColor, null, null);
        }

        protected override IEnumerable<MappingModel> GetOthersNotEqual()
        {
            yield return new MappingModel(null, Level.Error, ForeColor, BackColor, null, null);
            yield return new MappingModel(null, LevelConst, ConsoleColor.Cyan, BackColor, null, null);
            yield return new MappingModel(null, LevelConst, ForeColor, ConsoleColor.Cyan, null, null);
            yield return new MappingModel(null, Level.Error, ConsoleColor.Cyan, ConsoleColor.Cyan, null, null);
        }

        protected override int ExpectedHashCode
        {
            get
            {
                int hashCode = LevelConst != null ? LevelConst.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ ForeColor.GetHashCode();
                hashCode = (hashCode * 397) ^ BackColor.GetHashCode();
                return hashCode;
            }
        }
    }
}
