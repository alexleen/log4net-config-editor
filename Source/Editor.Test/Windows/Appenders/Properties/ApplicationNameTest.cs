// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Xml;
using Editor.Windows.Appenders.Properties;
using Editor.Windows.PropertyCommon;
using NUnit.Framework;

namespace Editor.Test.Windows.Appenders.Properties
{
    [TestFixture]
    public class ApplicationNameTest
    {
        private ApplicationName mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new ApplicationName(new ObservableCollection<IProperty>());
        }

        [Test]
        public void Load_ShouldSetCorrectValue()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender name=\"WindowsEventLog\" type=\"log4net.Appender.EventLogAppender\">\r\n    <param name=\"LogName\" value=\"SEL-5051\" />\r\n    <param name=\"ApplicationName\" value=\"SEL-5051\" />\r\n    <layout type=\"log4net.Layout.PatternLayout\">\r\n      <conversionPattern value=\"%message\" />\r\n    </layout>\r\n    <filter type=\"log4net.Filter.LoggerMatchFilter\">\r\n      <loggerToMatch value=\"WindowsEventLog\" />\r\n    </filter>\r\n    <filter type=\"log4net.Filter.LevelRangeFilter\">\r\n      <levelMin value=\"ERROR\" />\r\n    </filter>\r\n    <filter type=\"log4net.Filter.DenyAllFilter\" />\r\n  </appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual("SEL-5051", mSut.Value);
        }
    }
}
