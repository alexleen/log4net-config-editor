// Copyright © 2020 Alex Leendertsen

using System;
using System.Linq;
using System.Net.Mail;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.XML;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties.Base
{
    [TestFixture]
    public class EnumPropertyTest
    {
        private EnumProperty<MailPriority> mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new EnumProperty<MailPriority>("name", 100, "elementName");
        }

        [Test]
        public void Ctor_ShouldSelectFirstValue()
        {
            Assert.AreEqual(mSut.Values.First(), mSut.SelectedValue);
        }

        [Test]
        public void Ctor_ShouldUseStringValues()
        {
            CollectionAssert.AreEquivalent(Enum.GetNames(typeof(MailPriority)), mSut.Values);
        }

        [Test]
        public void Load_ShouldLoadValue()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("value", out _, "elementName").Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns(MailPriority.High.ToString());
                    ci[1] = result;
                    return true;
                });

            mSut.Load(config);

            Assert.AreEqual(MailPriority.High.ToString(), mSut.SelectedValue);
        }

        [Test]
        public void Load_ShouldNotLoadValuesItCannotParse()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("value", out _, "elementName").Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns("whatev"); //Not a valid value
                    ci[1] = result;
                    return true;
                });

            mSut.Load(config);

            Assert.AreEqual(mSut.Values.First(), mSut.SelectedValue);
        }

        [Test]
        public void Save_ShouldSave()
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            mSut.SelectedValue = MailPriority.High.ToString();

            mSut.Save(config);

            config.Received(1).Save(new Element("elementName", new[] { ("value", mSut.SelectedValue) }));
        }
    }
}
