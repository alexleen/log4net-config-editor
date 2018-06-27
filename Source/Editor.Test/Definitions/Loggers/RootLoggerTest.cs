using Editor.ConfigProperties;
using Editor.Definitions.Loggers;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System.Linq;
using System.Xml;

namespace Editor.Test.Definitions.Loggers
{
    [TestFixture]
    public class RootLoggerTest
    {
        private RootLogger mSut;

        [SetUp]
        public void SetUp()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement log4netNode = xmlDoc.CreateElement("log4net");

            IElementConfiguration configuration = Substitute.For<IElementConfiguration>();
            configuration.ConfigXml.Returns(xmlDoc);
            configuration.Log4NetNode.Returns(log4netNode);

            mSut = new RootLogger(configuration);
        }

        [Test]
        public void Name_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("Root Logger", mSut.Name);
        }

        [Test]
        public void Icon_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("pack://application:,,,/Editor;component/Images/text-x-log.png", mSut.Icon);
        }

        [Test]
        public void Initialize_ShouldAddDefaultProperties()
        {
            mSut.Initialize();

            mSut.Properties.Single(p => p.GetType() == typeof(LevelProperty));
            mSut.Properties.Single(p => p.GetType() == typeof(OutgoingRefs));
        }
    }
}
