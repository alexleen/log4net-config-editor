// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties.Base
{
    [TestFixture]
    public class NumericPropertyTest
    {
        private static NumericProperty<T> CreateSut<T>()
        {
            return new NumericProperty<T>("Num Prop:", "numProp", default(T));
        }

        [Test]
        public void Ctor_ShouldSetDefaultValue()
        {
            NumericProperty<int> sut = new NumericProperty<int>("Num Prop:", "numProp", 1234);

            Assert.AreEqual(sut.Value, 1234.ToString());
        }

        [Test]
        public void Ctor_ShouldNotSetDefaultValue_WhenNull()
        {
            NumericProperty<int> sut = CreateSut<int>();

            Assert.IsNull(sut.Value);
        }

        [TestCase(-32768)]
        [TestCase(0)]
        [TestCase(32767)]
        public void TryValidate_ShouldValidateShort(short value)
        {
            NumericProperty<short> sut = CreateSut<short>();

            sut.Value = value.ToString();

            Assert.IsTrue(sut.TryValidate(Substitute.For<IMessageBoxService>()));
        }

        [TestCase(-32769)]
        [TestCase(32768)]
        public void TryValidate_ShouldNotValidateShort(int value)
        {
            NumericProperty<short> sut = CreateSut<short>();

            sut.Value = value.ToString();

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();
            Assert.IsFalse(sut.TryValidate(messageBoxService));
            messageBoxService.Received(1).ShowError("'Num Prop' must be a valid Int16.");
        }

        [TestCase((ushort)0)]
        [TestCase((ushort)65535)]
        public void TryValidate_ShouldValidateUShort(ushort value)
        {
            NumericProperty<ushort> sut = CreateSut<ushort>();

            sut.Value = value.ToString();

            Assert.IsTrue(sut.TryValidate(Substitute.For<IMessageBoxService>()));
        }

        [TestCase(-1)]
        [TestCase(65536)]
        public void TryValidate_ShouldNotValidateUShort(int value)
        {
            NumericProperty<ushort> sut = CreateSut<ushort>();

            sut.Value = value.ToString();

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();
            Assert.IsFalse(sut.TryValidate(messageBoxService));
            messageBoxService.Received(1).ShowError("'Num Prop' must be a valid UInt16.");
        }

        [TestCase(-2147483648)]
        [TestCase(0)]
        [TestCase(2147483647)]
        public void TryValidate_ShouldValidateAnInt(int value)
        {
            NumericProperty<int> sut = CreateSut<int>();

            sut.Value = value.ToString();

            Assert.IsTrue(sut.TryValidate(Substitute.For<IMessageBoxService>()));
        }

        [TestCase(-2147483649)]
        [TestCase(2147483648)]
        public void TryValidate_ShouldNotValidateAnInt(long value)
        {
            NumericProperty<int> sut = CreateSut<int>();

            sut.Value = value.ToString();

            IMessageBoxService messageBoxService = Substitute.For<IMessageBoxService>();
            Assert.IsFalse(sut.TryValidate(messageBoxService));
            messageBoxService.Received(1).ShowError("'Num Prop' must be a valid Int32.");
        }

        [Test]
        public void Save_ShouldCreateAndAppendCorrectElement_WhenNotDefault()
        {
            NumericProperty<int> sut = CreateSut<int>();

            const string value = "10000";

            sut.Value = value;
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            sut.Save(config);

            config.Received(1).Save(("numProp", "value", value));
        }

        [Test]
        public void Save_ShouldNotCreateAndAppendElement_WhenDefault()
        {
            NumericProperty<int> sut = new NumericProperty<int>("Num Prop:", "numProp", 1234) { Value = "1234" };

            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            sut.Save(config);

            config.DidNotReceive().Save(Arg.Any<(string ElementName, string AttributeName, string AttributeValue)[]>());
        }

        [Test]
        public void Save_ShouldNotCreateAndAppendElement_WhenValueIsNull()
        {
            NumericProperty<int> sut = CreateSut<int>();

            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            sut.Save(config);

            config.DidNotReceive().Save(Arg.Any<(string ElementName, string AttributeName, string AttributeValue)[]>());
        }
    }
}
