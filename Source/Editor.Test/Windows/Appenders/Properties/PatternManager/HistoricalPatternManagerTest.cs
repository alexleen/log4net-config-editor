// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Linq;
using Editor.Properties;
using Editor.Windows.Appenders.Properties.PatternManager;
using NUnit.Framework;

namespace Editor.Test.Windows.Appenders.Properties.PatternManager
{
    [TestFixture]
    public class HistoricalPatternManagerTest
    {
        private const int Limit = 5;
        private IHistoricalPatternManager mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new HistoricalPatternManager();
        }

        [TearDown]
        public void TearDown()
        {
            Settings.Default.HistoricalPatterns = null;
        }

        [Test]
        public void GetPatterns_ShouldReturnEmpty_WhenNoPatternsExist()
        {
            CollectionAssert.IsEmpty(mSut.GetPatterns());
        }

        [Test]
        public void GetPatterns_ShouldReturnCorrectPattern()
        {
            Settings.Default.HistoricalPatterns = "<PatternsXml>\r\n" +
                                                  " <Patterns>\r\n" +
                                                  "  <StoredPattern LastUsage=\"2018-05-23T18:20:01.1761448Z\">\r\n" +
                                                  "   <Pattern>%message</Pattern>\r\n" +
                                                  "  </StoredPattern>\r\n" +
                                                  " </Patterns>\r\n" +
                                                  "</PatternsXml>";

            Assert.AreEqual(1, mSut.GetPatterns().Count());
            Assert.AreEqual("%message", mSut.GetPatterns().First());
        }

        [Test]
        public void GetPatterns_ShouldReturnOrderedPatterns()
        {
            Settings.Default.HistoricalPatterns = "<PatternsXml>\r\n" +
                                                  " <Patterns>\r\n" +
                                                  "  <StoredPattern LastUsage=\"2018-05-23T18:22:01.1761448Z\">\r\n" +
                                                  "   <Pattern>%message2</Pattern>\r\n" +
                                                  "  </StoredPattern>\r\n" +
                                                  "  <StoredPattern LastUsage=\"2018-05-23T18:21:01.1761448Z\">\r\n" +
                                                  "   <Pattern>%message1</Pattern>\r\n" +
                                                  "  </StoredPattern>\r\n" +
                                                  "  <StoredPattern LastUsage=\"2018-05-23T18:23:01.1761448Z\">\r\n" +
                                                  "   <Pattern>%message3</Pattern>\r\n" +
                                                  "  </StoredPattern>\r\n" +
                                                  " </Patterns>\r\n" +
                                                  "</PatternsXml>";

            Assert.AreEqual(3, mSut.GetPatterns().Count());

            IEnumerable<string> expectedOrder = new[] { "%message3", "%message2", "%message1" };

            CollectionAssert.AreEqual(expectedOrder, mSut.GetPatterns());
        }

        [Test]
        public void SavePattern_ShouldSavePattern_WhenNoPatternsExist()
        {
            mSut.SavePattern("%message");

            Assert.AreEqual(1, mSut.GetPatterns().Count());
            Assert.AreEqual("%message", mSut.GetPatterns().First());
        }

        [Test]
        public void SavePattern_ShouldAddPatternToExistingPatterns()
        {
            Settings.Default.HistoricalPatterns = "<PatternsXml>\r\n" +
                                                  " <Patterns>\r\n" +
                                                  "  <StoredPattern LastUsage=\"2018-05-23T18:22:01.1761448Z\">\r\n" +
                                                  "   <Pattern>%message2</Pattern>\r\n" +
                                                  "  </StoredPattern>\r\n" +
                                                  "  <StoredPattern LastUsage=\"2018-05-23T18:21:01.1761448Z\">\r\n" +
                                                  "   <Pattern>%message1</Pattern>\r\n" +
                                                  "  </StoredPattern>\r\n" +
                                                  "  <StoredPattern LastUsage=\"2018-05-23T18:23:01.1761448Z\">\r\n" +
                                                  "   <Pattern>%message3</Pattern>\r\n" +
                                                  "  </StoredPattern>\r\n" +
                                                  " </Patterns>\r\n" +
                                                  "</PatternsXml>";

            mSut.SavePattern("%message4");

            Assert.AreEqual(4, mSut.GetPatterns().Count());

            IEnumerable<string> expectedOrder = new[] { "%message4", "%message3", "%message2", "%message1" };

            CollectionAssert.AreEqual(expectedOrder, mSut.GetPatterns());
        }

        [Test]
        public void SavePattern_ShouldOnlySaveMostRecentPatternsWithinLimit()
        {
            Settings.Default.HistoricalPatterns = "<PatternsXml>\r\n" +
                                                  " <Patterns>\r\n" +
                                                  "  <StoredPattern LastUsage=\"2018-05-23T18:22:01.1761448Z\">\r\n" +
                                                  "   <Pattern>%message2</Pattern>\r\n" +
                                                  "  </StoredPattern>\r\n" +
                                                  "  <StoredPattern LastUsage=\"2018-05-23T18:21:01.1761448Z\">\r\n" +
                                                  "   <Pattern>%message1</Pattern>\r\n" +
                                                  "  </StoredPattern>\r\n" +
                                                  "  <StoredPattern LastUsage=\"2018-05-23T18:23:01.1761448Z\">\r\n" +
                                                  "   <Pattern>%message3</Pattern>\r\n" +
                                                  "  </StoredPattern>\r\n" +
                                                  "  <StoredPattern LastUsage=\"2018-05-23T18:24:01.1761448Z\">\r\n" +
                                                  "   <Pattern>%message4</Pattern>\r\n" +
                                                  "  </StoredPattern>\r\n" +
                                                  "  <StoredPattern LastUsage=\"2018-05-23T18:25:01.1761448Z\">\r\n" +
                                                  "   <Pattern>%message5</Pattern>\r\n" +
                                                  "  </StoredPattern>\r\n" +
                                                  " </Patterns>\r\n" +
                                                  "</PatternsXml>";

            mSut.SavePattern("%message6");

            Assert.AreEqual(Limit, mSut.GetPatterns().Count());

            IEnumerable<string> expectedOrder = new[] { "%message6", "%message5", "%message4", "%message3", "%message2" };

            CollectionAssert.AreEqual(expectedOrder, mSut.GetPatterns());
        }

        [Test]
        public void SavePattern_ShouldNotSaveDuplicates()
        {
            Settings.Default.HistoricalPatterns = "<PatternsXml>\r\n" +
                                                  " <Patterns>\r\n" +
                                                  "  <StoredPattern LastUsage=\"2018-05-23T18:20:01.1761448Z\">\r\n" +
                                                  "   <Pattern>%message</Pattern>\r\n" +
                                                  "  </StoredPattern>\r\n" +
                                                  " </Patterns>\r\n" +
                                                  "</PatternsXml>";

            mSut.SavePattern("%message");

            Assert.AreEqual(1, mSut.GetPatterns().Count());
            Assert.AreEqual("%message", mSut.GetPatterns().First());
        }

        [Test]
        public void SavePattern_ShouldUpdateLastUsageOnDuplicate()
        {
            Settings.Default.HistoricalPatterns = "<PatternsXml>\r\n" +
                                                  " <Patterns>\r\n" +
                                                  "  <StoredPattern LastUsage=\"2018-05-23T18:20:01.1761448Z\">\r\n" +
                                                  "   <Pattern>%message</Pattern>\r\n" +
                                                  "  </StoredPattern>\r\n" +
                                                  "  <StoredPattern LastUsage=\"2018-05-23T18:21:01.1761448Z\">\r\n" +
                                                  "   <Pattern>%message1</Pattern>\r\n" +
                                                  "  </StoredPattern>\r\n" +
                                                  " </Patterns>\r\n" +
                                                  "</PatternsXml>";

            //Test sanity check
            Assert.AreEqual(2, mSut.GetPatterns().Count());
            Assert.AreEqual("%message1", mSut.GetPatterns().First());

            mSut.SavePattern("%message");

            Assert.AreEqual(2, mSut.GetPatterns().Count());
            Assert.AreEqual("%message", mSut.GetPatterns().First());
        }
    }
}
