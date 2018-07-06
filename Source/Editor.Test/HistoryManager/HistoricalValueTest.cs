// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using Editor.HistoryManager;
using Editor.Test.TestUtilities;
using NUnit.Framework;

namespace Editor.Test.HistoryManager
{
    [TestFixture]
    public class HistoricalValueTest : EqualityTests<HistoricalValue>
    {
        protected override HistoricalValue GetSut()
        {
            return new HistoricalValue { Value = "value" };
        }

        protected override HistoricalValue GetOtherEqual()
        {
            return new HistoricalValue { Value = "value" };
        }

        protected override IEnumerable<HistoricalValue> GetOthersNotEqual()
        {
            yield return new HistoricalValue { Value = "value2" };
        }

        protected override int ExpectedHashCode => Sut.Value.GetHashCode();
    }
}
