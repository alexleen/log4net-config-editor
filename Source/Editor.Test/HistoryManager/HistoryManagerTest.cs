// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Linq;
using Editor.HistoryManager;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.HistoryManager
{
    [TestFixture]
    public class HistoryManagerTest
    {
        private const int Limit = 5;
        private IHistoryManager mSut;
        private ISettingManager<string> mSettingManager;
        private string mSettingName;

        [SetUp]
        public void SetUp()
        {
            mSettingManager = Substitute.For<ISettingManager<string>>();
            mSettingName = "whatev";
            mSut = new Editor.HistoryManager.HistoryManager(mSettingName, mSettingManager);
        }

        [Test]
        public void Get_ShouldReturnEmpty_WhenNoValuesExist()
        {
            CollectionAssert.IsEmpty(mSut.Get());
        }

        [Test]
        public void Get_ShouldReturnCorrectValue()
        {
            mSettingManager.Get(mSettingName).Returns("<History xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\n" +
                                                      "  <HistoricalValue LastUsage=\"2018-05-26T02:54:42.0346884Z\">%message</HistoricalValue>\n" +
                                                      "</History>");

            Assert.AreEqual(1, mSut.Get().Count());
            Assert.AreEqual("%message", mSut.Get().First());
        }

        [Test]
        public void Get_ShouldReturnOrderedValues()
        {
            mSettingManager.Get(mSettingName).Returns("<History xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\n" +
                                                      "  <HistoricalValue LastUsage=\"2018-05-23T18:22:01.1761448Z\">%message2</HistoricalValue>\n" +
                                                      "  <HistoricalValue LastUsage=\"2018-05-23T18:21:01.1761448Z\">%message1</HistoricalValue>\n" +
                                                      "  <HistoricalValue LastUsage=\"2018-05-23T18:23:01.1761448Z\">%message3</HistoricalValue>\n" +
                                                      "</History>");

            Assert.AreEqual(3, mSut.Get().Count());

            IEnumerable<string> expectedOrder = new[] { "%message3", "%message2", "%message1" };

            CollectionAssert.AreEqual(expectedOrder, mSut.Get());
        }

        [Test]
        public void Set_ShouldSet_WhenNoValuesExist()
        {
            SetUpSettingsManager(null);

            mSut.Save("%message");

            Assert.AreEqual(1, mSut.Get().Count());
            Assert.AreEqual("%message", mSut.Get().First());
        }

        [Test]
        public void Set_ShouldAddValueToExistingValues()
        {
            SetUpSettingsManager("<History xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\n" +
                                 "  <HistoricalValue LastUsage=\"2018-05-23T18:22:01.1761448Z\">%message2</HistoricalValue>\n" +
                                 "  <HistoricalValue LastUsage=\"2018-05-23T18:21:01.1761448Z\">%message1</HistoricalValue>\n" +
                                 "  <HistoricalValue LastUsage=\"2018-05-23T18:23:01.1761448Z\">%message3</HistoricalValue>\n" +
                                 "</History>");

            mSut.Save("%message4");

            Assert.AreEqual(4, mSut.Get().Count());

            IEnumerable<string> expectedOrder = new[] { "%message4", "%message3", "%message2", "%message1" };

            CollectionAssert.AreEqual(expectedOrder, mSut.Get());
        }

        [Test]
        public void Set_ShouldOnlySaveMostRecentValuesWithinLimit()
        {
            SetUpSettingsManager("<History xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\n" +
                                 "  <HistoricalValue LastUsage=\"2018-05-23T18:22:01.1761448Z\">%message2</HistoricalValue>\n" +
                                 "  <HistoricalValue LastUsage=\"2018-05-23T18:21:01.1761448Z\">%message1</HistoricalValue>\n" +
                                 "  <HistoricalValue LastUsage=\"2018-05-23T18:23:01.1761448Z\">%message3</HistoricalValue>\n" +
                                 "  <HistoricalValue LastUsage=\"2018-05-23T18:24:01.1761448Z\">%message4</HistoricalValue>\n" +
                                 "  <HistoricalValue LastUsage=\"2018-05-23T18:25:01.1761448Z\">%message5</HistoricalValue>\n" +
                                 "</History>");

            mSut.Save("%message6");

            Assert.AreEqual(Limit, mSut.Get().Count());

            IEnumerable<string> expectedOrder = new[] { "%message6", "%message5", "%message4", "%message3", "%message2" };

            CollectionAssert.AreEqual(expectedOrder, mSut.Get());
        }

        [Test]
        public void Set_ShouldNotSaveDuplicates()
        {
            mSettingManager.Get(mSettingName).Returns("<History xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\n" +
                                                      "  <HistoricalValue LastUsage=\"2018-05-26T02:54:42.0346884Z\">%message</HistoricalValue>\n" +
                                                      "</History>");

            mSut.Save("%message");

            Assert.AreEqual(1, mSut.Get().Count());
            Assert.AreEqual("%message", mSut.Get().First());
        }

        [Test]
        public void Set_ShouldUpdateLastUsageOnDuplicate()
        {
            SetUpSettingsManager("<History xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\n" +
                                 "  <HistoricalValue LastUsage=\"2018-05-23T18:20:01.1761448Z\">%message</HistoricalValue>\n" +
                                 "  <HistoricalValue LastUsage=\"2018-05-23T18:21:01.1761448Z\">%message1</HistoricalValue>\n" +
                                 "</History>");

            //Test sanity check
            Assert.AreEqual(2, mSut.Get().Count());
            Assert.AreEqual("%message1", mSut.Get().First());

            mSut.Save("%message");

            Assert.AreEqual(2, mSut.Get().Count());
            Assert.AreEqual("%message", mSut.Get().First());
        }

        private void SetUpSettingsManager(string initialXml)
        {
            string xml = initialXml;

            mSettingManager.Get(mSettingName).Returns(ci => xml);

            mSettingManager.Set(mSettingName, Arg.Do<string>(value => xml = value));
        }
    }
}
