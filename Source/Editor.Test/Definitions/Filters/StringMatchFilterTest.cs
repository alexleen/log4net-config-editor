// Copyright © 2018 Alex Leendertsen

using System.Linq;
using Editor.ConfigProperties;
using Editor.ConfigProperties.Base;
using Editor.Definitions.Filters;
using Editor.Descriptors;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.Definitions.Filters
{
    [TestFixture]
    public class StringMatchFilterTest
    {
        private StringMatchFilter mSut;
        private IMessageBoxService mMessageBoxService;

        [SetUp]
        public void SetUp()
        {
            mSut = new StringMatchFilter();
            mMessageBoxService = Substitute.For<IMessageBoxService>();
            mSut.MessageBoxService = mMessageBoxService;
        }

        [Test]
        public void Name_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("String Match Filter", mSut.Name);
        }

        [Test]
        public void Icon_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("pack://application:,,,/Editor;component/Images/view-filter.png", mSut.Icon);
        }

        [Test]
        public void Descriptor_ShouldReturnCorrectValue()
        {
            Assert.AreEqual(FilterDescriptor.String, mSut.Descriptor);
        }

        [Test]
        public void Initialize_ShouldAddTheCorrectNumberOfProperties()
        {
            mSut.Initialize();

            Assert.AreEqual(3, mSut.Properties.Count);
        }

        [Test]
        public void Initialize_ShouldAddStringProperties()
        {
            mSut.Initialize();

            mSut.Properties.Single(p => p.GetType() == typeof(BooleanPropertyBase));
            mSut.Properties.Single(p => p.GetType() == typeof(StringMatch));
            mSut.Properties.Single(p => p.GetType() == typeof(RegexMatch));
        }

        [Test]
        public void Initialize_ShouldSetIsFocusedToTrue()
        {
            mSut.Initialize();

            StringMatch stringMatch = (StringMatch)mSut.Properties.Single(p => p.GetType() == typeof(StringMatch));

            Assert.IsTrue(stringMatch.IsFocused);
        }

        [TestCase(null, null, 1)]
        [TestCase("", null, 1)]
        [TestCase(null, "", 1)]
        [TestCase("", "", 1)]
        [TestCase(null, "something", 0)]
        [TestCase("something", null, 0)]
        public void Sut_ShouldCoordinateValidation(string strMatch, string regexMatchStr, int expectedMessageBoxCalls)
        {
            mSut.Initialize();

            StringMatch stringMatch = (StringMatch)mSut.Properties.Single(p => p.GetType() == typeof(StringMatch));
            RegexMatch regexMatch = (RegexMatch)mSut.Properties.Single(p => p.GetType() == typeof(RegexMatch));

            stringMatch.Value = strMatch;
            regexMatch.Value = regexMatchStr;

            stringMatch.TryValidate(mMessageBoxService);

            mMessageBoxService.Received(expectedMessageBoxCalls).ShowError("Either 'String to Match' or 'Regex to Match' must be specified.");
        }
    }
}
