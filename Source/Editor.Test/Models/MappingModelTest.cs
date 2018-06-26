// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using Editor.Models;
using Editor.Test.TestUtilities;
using log4net.Core;
using NUnit.Framework;

namespace Editor.Test.Models
{
    [TestFixture]
    public class MappingModelTest //: EqualityTests<MappingModel>
    {
        private static readonly Level sLevel = Level.All;
        private const ConsoleColor ForeColor = ConsoleColor.Black;
        private const ConsoleColor BackColor = ConsoleColor.Blue;

        //[Test]
        //public void ToString_ShouldReturnCorrectString()
        //{
        //    Assert.AreEqual($"{sLevel} | {ForeColor} | {BackColor}", Sut.ToString());
        //}

        //protected override MappingModel GetSut()
        //{
        //    return new MappingModel(null, null, null, null);
        //}

        //protected override MappingModel GetOtherEqual()
        //{
        //    return new MappingModel(null, null, null, null);
        //}

        //protected override IEnumerable<MappingModel> GetOthersNotEqual()
        //{
        //    yield return new MappingModel(null, null, null, null);
        //    yield return new MappingModel(null, null, null, null);
        //    yield return new MappingModel(null, null, null, null);
        //    yield return new MappingModel(null, null, null, null);
        //}

        //protected override int ExpectedHashCode
        //{
        //    get
        //    {
        //        int hashCode = sLevel != null ? sLevel.GetHashCode() : 0;
        //        hashCode = (hashCode * 397) ^ ForeColor.GetHashCode();
        //        hashCode = (hashCode * 397) ^ BackColor.GetHashCode();
        //        return hashCode;
        //    }
        //}
    }
}
