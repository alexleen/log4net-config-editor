using Editor.Interfaces;
using Editor.Test.TestUtilities;
using NUnit.Framework;
using System.Collections.Generic;

namespace Editor.Test.Windows.SizeLocation
{
    [TestFixture]
    public class SizeTest : EqualityTests<Size>
    {
        protected override Size GetSut()
        {
            return new Size(1, 2, 3);
        }

        protected override Size GetOtherEqual()
        {
            return new Size(1, 2, 3);
        }

        protected override IEnumerable<Size> GetOthersNotEqual()
        {
            yield return new Size(2, 2, 3);
            yield return new Size(1, 3, 3);
            yield return new Size(1, 2, 4);
            yield return new Size(2, 3, 4);
        }

        protected override int ExpectedHashCode => 1165492224;

        [Test]
        public void ToString_ShouldReturnCorrectString()
        {
            Assert.AreEqual("3 <- 1 -> 2", Sut.ToString());
        }
    }
}
