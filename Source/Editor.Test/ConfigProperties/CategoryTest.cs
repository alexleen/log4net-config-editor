// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.XML;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class CategoryTest
    {
        private const string DefaultValue = "%logger";
        private Category mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new Category();
        }

        [Test]
        public void Name_ShouldBeInitializedCorrectly()
        {
            Assert.AreEqual("Category:", mSut.Name);
        }

        [Test]
        public void Value_ShouldBeInitializedToDefault()
        {
            Assert.AreEqual(DefaultValue, mSut.Value);
        }

        [Test]
        public void Load_ShouldLoadCorrectValue()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("value", out _, "category", "conversionPattern").Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns("%logger %date");
                    ci[1] = result;
                    return true;
                });

            mSut.Load(config);

            Assert.AreEqual("%logger %date", mSut.Value);
        }

        [Test]
        public void Load_ShouldNotLoad_WhenNoCategoryExists()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("value", out _, "category", "conversionPattern").Returns(false);

            mSut.Load(config);

            Assert.AreEqual(DefaultValue, mSut.Value);
        }

        [TestCase(null)]
        [TestCase("")]
        public void Load_ShouldNotLoad_WhenNoConversionPatternExists(string attrValue)
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("value", out _, "category", "conversionPattern").Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns(attrValue);
                    ci[1] = result;
                    return true;
                });

            mSut.Load(config);

            Assert.AreEqual(DefaultValue, mSut.Value);
        }

        [Test]
        public void Save_ShouldSaveCorrectValue()
        {
            mSut.Value = "whatev";

            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.Save(config);

            config.Received(1).SaveHierarchical(new Element("category", new[] { ("type", LayoutDescriptor.Pattern.TypeNamespace) }), new Element("conversionPattern", new[] { ("value", mSut.Value) }));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("%logger")]
        public void Save_ShouldNotSave(string value)
        {
            mSut.Value = value;

            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.Save(config);

            config.DidNotReceiveWithAnyArgs().Save();
        }
    }
}
