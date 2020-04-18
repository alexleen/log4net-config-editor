// Copyright © 2020 Alex Leendertsen

using System.Collections.Generic;
using System.Linq;
using Editor.ConfigProperties;
using Editor.Interfaces;
using Editor.Models;
using log4net.Core;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class FixTest
    {
        [SetUp]
        public void SetUp()
        {
            mSut = new Fix();
        }

        private Fix mSut;

        [TestCase(Fix.NonePreset, FixFlags.None)]
        [TestCase(Fix.PartialPreset, FixFlags.Partial)]
        [TestCase(Fix.AllPreset, FixFlags.Message | FixFlags.ThreadName | FixFlags.LocationInfo | FixFlags.UserName | FixFlags.Domain | FixFlags.Identity | FixFlags.Exception | FixFlags.Properties)]
        [TestCase(Fix.CustomPreset, FixFlags.None)] //None be default, so setting to custom should no effect
        [TestCase("other", FixFlags.None)]
        public void SelectedPreset_ShouldConfigureFixesCorrecly(string value, FixFlags expected)
        {
            mSut.SelectedPreset = value;

            FixFlags enabled = mSut.Fixes.Where(fix => fix.Enabled).Aggregate(FixFlags.None, (current, fix) => current | fix.Flag);

            Assert.AreEqual(expected, enabled);
        }

        private static readonly IEnumerable<TestCaseData> sFixTests = new[]
        {
            //TODO False cases
            // new TestCaseData(null, FixFlags.None, Fix.NonePreset),
            // new TestCaseData("<Fix />", FixFlags.None, Fix.NonePreset),
            new TestCaseData("", FixFlags.None, Fix.NonePreset),
            new TestCaseData("0", FixFlags.None, Fix.NonePreset),
            new TestCaseData("0x02", FixFlags.None, Fix.NonePreset), //Hex format is not supported by this tool
            new TestCaseData("10", FixFlags.ThreadName, Fix.CustomPreset),
            new TestCaseData("96", FixFlags.UserName | FixFlags.Domain, Fix.CustomPreset),
            new TestCaseData("512", FixFlags.Properties, Fix.CustomPreset),
            new TestCaseData("844", FixFlags.Partial, Fix.PartialPreset),
            new TestCaseData("268435455", FixFlags.Message | FixFlags.ThreadName | FixFlags.LocationInfo | FixFlags.UserName | FixFlags.Domain | FixFlags.Identity | FixFlags.Exception | FixFlags.Properties, Fix.AllPreset),
            new TestCaseData("str", FixFlags.None, Fix.NonePreset)
        };

        [TestCaseSource(nameof(sFixTests))]
        public void Load_ShouldLoadPresetCorrectly(string value, FixFlags expectedFlags, string expectedPreset)
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("value", out _, "Fix").Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns(value);
                    ci[1] = result;
                    return true;
                });

            mSut.Load(config);

            Assert.AreEqual(expectedPreset, mSut.SelectedPreset);
        }

        [TestCaseSource(nameof(sFixTests))]
        public void Load_ShouldLoadFixesCorrectly(string value, FixFlags expectedFlags, string expectedPreset)
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();
            config.Load("value", out _, "Fix").Returns(ci =>
                {
                    IValueResult result = Substitute.For<IValueResult>();
                    result.AttributeValue.Returns(value);
                    ci[1] = result;
                    return true;
                });

            mSut.Load(config);

            FixFlags enabled = mSut.Fixes.Where(fix => fix.Enabled).Aggregate(FixFlags.None, (current, fix) => current | fix.Flag);

            Assert.AreEqual(expectedFlags, enabled);
        }

        [TestCase(FixFlags.None)]
        [TestCase(FixFlags.Partial)]
        [TestCase(FixFlags.Message | FixFlags.ThreadName | FixFlags.LocationInfo | FixFlags.UserName | FixFlags.Domain | FixFlags.Identity | FixFlags.Exception | FixFlags.Properties)]
        public void Save_ShouldSaveCorrectly(FixFlags flags)
        {
            IElementConfiguration config = Substitute.For<IElementConfiguration>();

            foreach (FixModel fixModel in mSut.Fixes)
            {
                fixModel.Enabled = flags.HasFlag(fixModel.Flag);
            }

            mSut.Save(config);

            config.Received(1).Save(("Fix", "value", ((int)flags).ToString()));
        }

        [Test]
        public void Fixes_ShouldBeInitializedCorrectly()
        {
            CollectionAssert.AreEqual(new[]
                                      {
                                          new FixModel(FixFlags.Message, false),
                                          new FixModel(FixFlags.ThreadName, false),
                                          new FixModel(FixFlags.LocationInfo, true, "Possible performance degradation"),
                                          new FixModel(FixFlags.UserName, true, "Possible performance degradation"),
                                          new FixModel(FixFlags.Domain, false),
                                          new FixModel(FixFlags.Identity, true, "Possible performance degradation"),
                                          new FixModel(FixFlags.Exception, false),
                                          new FixModel(FixFlags.Properties, false)
                                      },
                                      mSut.Fixes);
        }

        [Test]
        public void Presets_ShouldBeInitializedCorrectly()
        {
            CollectionAssert.AreEqual(new[] { Fix.NonePreset, Fix.PartialPreset, Fix.AllPreset, Fix.CustomPreset }, mSut.Presets);
        }

        [Test]
        public void SelectedPreset_ShouldBeInitializedToNone()
        {
            Assert.AreEqual(Fix.NonePreset, mSut.SelectedPreset);
        }

        [Test]
        public void SelectedPreset_ShouldFirePropChange_WhenChanged()
        {
            bool fired = false;
            mSut.PropertyChanged += (sender, args) => { fired = true; };

            mSut.SelectedPreset = "other";

            Assert.IsTrue(fired);
        }

        [Test]
        public void SelectedPreset_ShouldNotFirePropChange_WhenUnchanged()
        {
            bool fired = false;
            mSut.PropertyChanged += (sender, args) => { fired = true; };

            mSut.SelectedPreset = mSut.SelectedPreset;

            Assert.IsFalse(fired);
        }
    }
}
