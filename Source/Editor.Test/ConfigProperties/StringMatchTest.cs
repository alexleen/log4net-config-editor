// Copyright © 2020 Alex Leendertsen

using System.Xml;
using Editor.ConfigProperties;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class StringMatchTest
    {
        [SetUp]
        public void SetUp()
        {
            mValidateCalled = false;
            mSut = new StringMatch(Validate);
        }

        private bool mValidateCalled;
        private StringMatch mSut;

        private bool Validate()
        {
            mValidateCalled = true;
            return false;
        }

        //TODO move to ElementConfigurationTest
        [TestCase(null, null)]
        [TestCase("<stringToMatch />", null)]
        [TestCase("<stringToMatch value=\"\" />", null)]
        [TestCase("<stringToMatch value=\"str\" />", "str")]
        public void Load_ShouldLoadCorrectly(string xml, string expected)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\n" +
                           $"      {xml}\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(expected, mSut.Value);
        }

        [TestCase(null)]
        [TestCase("")]
        public void Save_ShouldNotSaveStringMatch(string value)
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.Value = value;
            mSut.Save(config);

            config.DidNotReceive().Save(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
            config.DidNotReceive().Save(Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public void Save_ShouldSave()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.Value = "match";
            mSut.Save(config);

            config.Received(1).Save("stringToMatch", "value", mSut.Value);
        }

        [Test]
        public void TryValidate_ShouldCallValidate()
        {
            //Test sanity check
            Assert.IsFalse(mValidateCalled);

            mSut.TryValidate(Substitute.For<IMessageBoxService>());

            Assert.IsTrue(mValidateCalled);
        }
    }
}
