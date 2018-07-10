// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using Editor.Test.TestUtilities;
using Editor.Windows.SizeLocation;
using NUnit.Framework;

namespace Editor.Test.Windows.SizeLocation
{
    [TestFixture]
    public class SizeTest_Default : EqualityTests<Size>
    {
        protected override Size GetSut()
        {
            return new Size();
        }

        protected override Size GetOtherEqual()
        {
            return new Size();
        }

        protected override IEnumerable<Size> GetOthersNotEqual()
        {
            yield return new Size(double.Epsilon);
            yield return new Size(max: double.NegativeInfinity);
            yield return new Size(min: 1);
            yield return new Size(double.Epsilon, double.NegativeInfinity, 1);
        }

        protected override int ExpectedHashCode => -1119354880;
    }
}
