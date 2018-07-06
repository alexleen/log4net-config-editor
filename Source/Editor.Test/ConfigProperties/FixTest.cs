// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Interfaces;
using Editor.Models;
using log4net.Core;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class FixTest
    {
        private Fix mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new Fix(new ReadOnlyCollection<IProperty>(new List<IProperty>()));
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
        public void SelectedPreset_ShouldNotFirePropChange_WhenUnchanged()
        {
            bool fired = false;
            mSut.PropertyChanged += (sender, args) => { fired = true; };

            mSut.SelectedPreset = mSut.SelectedPreset;

            Assert.IsFalse(fired);
        }

        [Test]
        public void SelectedPreset_ShouldFirePropChange_WhenChanged()
        {
            bool fired = false;
            mSut.PropertyChanged += (sender, args) => { fired = true; };

            mSut.SelectedPreset = "other";

            Assert.IsTrue(fired);
        }

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
            new TestCaseData(null, FixFlags.None, Fix.NonePreset),
            new TestCaseData("<Fix />", FixFlags.None, Fix.NonePreset),
            new TestCaseData("<Fix value=\"\" />", FixFlags.None, Fix.NonePreset),
            new TestCaseData("<Fix value=\"0\" />", FixFlags.None, Fix.NonePreset),
            new TestCaseData("<Fix value=\"0x02\" />", FixFlags.None, Fix.NonePreset), //Hex format is not supported by this tool
            new TestCaseData("<Fix value=\"10\" />", FixFlags.ThreadName, Fix.CustomPreset),
            new TestCaseData("<Fix value=\"96\" />", FixFlags.UserName | FixFlags.Domain, Fix.CustomPreset),
            new TestCaseData("<Fix value=\"512\" />", FixFlags.Properties, Fix.CustomPreset),
            new TestCaseData("<Fix value=\"844\" />", FixFlags.Partial, Fix.PartialPreset),
            new TestCaseData("<Fix value=\"268435455\" />", FixFlags.Message | FixFlags.ThreadName | FixFlags.LocationInfo | FixFlags.UserName | FixFlags.Domain | FixFlags.Identity | FixFlags.Exception | FixFlags.Properties, Fix.AllPreset),
            new TestCaseData("<Fix value=\"str\" />", FixFlags.None, Fix.NonePreset)
        };

        [TestCaseSource(nameof(sFixTests))]
        public void Load_ShouldLoadPresetCorrectly(string xml, FixFlags expectedFlags, string expectedPreset)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender name=\"asyncAppender\" type=\"Log4Net.Async.AsyncForwardingAppender,Log4Net.Async\">\n" +
                           $"  {xml}\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            Assert.AreEqual(expectedPreset, mSut.SelectedPreset);
        }

        [TestCaseSource(nameof(sFixTests))]
        public void Load_ShouldLoadFixesCorrectly(string xml, FixFlags expectedFlags, string expectedPreset)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender name=\"asyncAppender\" type=\"Log4Net.Async.AsyncForwardingAppender,Log4Net.Async\">\n" +
                           $"  {xml}\n" +
                           "</appender>");

            mSut.Load(xmlDoc.FirstChild);

            FixFlags enabled = mSut.Fixes.Where(fix => fix.Enabled).Aggregate(FixFlags.None, (current, fix) => current | fix.Flag);

            Assert.AreEqual(expectedFlags, enabled);
        }

        [TestCase(FixFlags.None)]
        [TestCase(FixFlags.Partial)]
        [TestCase(FixFlags.Message | FixFlags.ThreadName | FixFlags.LocationInfo | FixFlags.UserName | FixFlags.Domain | FixFlags.Identity | FixFlags.Exception | FixFlags.Properties)]
        public void Save_ShouldSaveCorrectly(FixFlags flags)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            foreach (FixModel fixModel in mSut.Fixes)
            {
                fixModel.Enabled = flags.HasFlag(fixModel.Flag);
            }

            mSut.Save(xmlDoc, appender);

            XmlNode fixNode = appender.SelectSingleNode("Fix");

            Assert.IsNotNull(fixNode);
            Assert.AreEqual(((int)flags).ToString(), fixNode.Attributes?["value"].Value);
        }
    }
}
