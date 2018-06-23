// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Xml;
using Editor.ConfigProperties;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Models;
using Editor.Utilities;
using Editor.Windows;
using NSubstitute;
using NUnit.Framework;

namespace Editor.Test.ConfigProperties
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class FiltersTest
    {
        private Filters mSut;
        private XmlDocument mXmlDoc;
        private XmlElement mAppender;
        private IMessageBoxService mMessageBoxService;

        [SetUp]
        public void SetUp()
        {
            mXmlDoc = new XmlDocument();
            mAppender = mXmlDoc.CreateElement("appender");
            IConfiguration configuration = Substitute.For<IConfiguration>();
            configuration.ConfigXml.Returns(mXmlDoc);
            mMessageBoxService = Substitute.For<IMessageBoxService>();
            mSut = new Filters(new ReadOnlyCollection<IProperty>(new List<IProperty>()), configuration, mMessageBoxService);
        }

        [Test]
        public void AvailableFilters_ShouldBeInitializedCorrectly()
        {
            IEnumerable<FilterDescriptor> expectedFilters = new[]
            {
                FilterDescriptor.DenyAll,
                FilterDescriptor.LevelMatch,
                FilterDescriptor.LevelRange,
                FilterDescriptor.LoggerMatch,
                FilterDescriptor.String,
                FilterDescriptor.Property
            };

            CollectionAssert.AreEqual(expectedFilters, mSut.AvailableFilters);
        }

        [Test]
        public void ExistingFilters_ShouldBeInitializedCorrectly()
        {
            CollectionAssert.IsEmpty(mSut.ExistingFilters);
        }

        [Test]
        public void AddFilter_ShouldAddDenyAllFilter()
        {
            mSut.AddFilter.Execute(FilterDescriptor.DenyAll);

            Assert.AreEqual(1, mSut.ExistingFilters.Count);
            Assert.AreEqual(FilterDescriptor.DenyAll, mSut.ExistingFilters.First().Descriptor);
        }

        [TestCase("log4net.Filter.LevelMatchFilter")]
        [TestCase("log4net.Filter.LevelRangeFilter")]
        [TestCase("log4net.Filter.LoggerMatchFilter")]
        [TestCase("log4net.Filter.PropertyFilter")]
        [TestCase("log4net.Filter.StringMatchFilter")]
        public void AddFilter_ShouldShowElementWindow(string filterType)
        {
            FilterDescriptor.TryFindByTypeNamespace(filterType, out FilterDescriptor descriptor);
            mSut.AddFilter.Execute(descriptor);

            mMessageBoxService.Received(1).ShowWindow(Arg.Any<ElementWindow>());
        }

        [Test]
        public void HelpCommand_ShouldShowHelp()
        {
            mSut.Help.Execute(null);

            mMessageBoxService.Received(1).ShowInformation(Arg.Any<string>());
        }

        [Test]
        public void Remove_ShouldRemoveFilter()
        {
            mSut.AddFilter.Execute(FilterDescriptor.DenyAll);
            mSut.ExistingFilters.First().Remove.Execute(null);

            CollectionAssert.IsEmpty(mSut.ExistingFilters);
        }

        [Test]
        public void MoveUp_ShouldDecrementFilterIndex()
        {
            mSut.AddFilter.Execute(FilterDescriptor.DenyAll);
            mSut.AddFilter.Execute(FilterDescriptor.DenyAll);

            Tuple<FilterModel, int> first = Tuple.Create(mSut.ExistingFilters.First(), 0);
            Tuple<FilterModel, int> second = Tuple.Create(mSut.ExistingFilters.Skip(1).First(), 1);

            //Test sanity check
            Assert.AreEqual(0, mSut.ExistingFilters.IndexOf(first.Item1));
            Assert.AreEqual(1, mSut.ExistingFilters.IndexOf(second.Item1));

            second.Item1.MoveUp.Execute(null);

            Assert.AreEqual(1, mSut.ExistingFilters.IndexOf(first.Item1));
            Assert.AreEqual(0, mSut.ExistingFilters.IndexOf(second.Item1));
        }

        [Test]
        public void MoveDown_ShouldIncrementFilterIndex()
        {
            mSut.AddFilter.Execute(FilterDescriptor.DenyAll);
            mSut.AddFilter.Execute(FilterDescriptor.DenyAll);

            Tuple<FilterModel, int> first = Tuple.Create(mSut.ExistingFilters.First(), 0);
            Tuple<FilterModel, int> second = Tuple.Create(mSut.ExistingFilters.Skip(1).First(), 1);

            //Test sanity check
            Assert.AreEqual(0, mSut.ExistingFilters.IndexOf(first.Item1));
            Assert.AreEqual(1, mSut.ExistingFilters.IndexOf(second.Item1));

            first.Item1.MoveDown.Execute(null);

            Assert.AreEqual(1, mSut.ExistingFilters.IndexOf(first.Item1));
            Assert.AreEqual(0, mSut.ExistingFilters.IndexOf(second.Item1));
        }

        [Test]
        public void Load_ShouldNotLoadFilters_WhenThereAreNone()
        {
            mSut.Load(mAppender);

            CollectionAssert.IsEmpty(mSut.ExistingFilters);
        }

        [Test]
        public void Load_ShouldLoadCorrectFilters()
        {
            mXmlDoc.CreateElementWithAttribute("filter", "type", "sometype").AppendTo(mAppender);
            mXmlDoc.CreateElementWithAttribute("filter", "type", FilterDescriptor.DenyAll.TypeNamespace).AppendTo(mAppender);
            mXmlDoc.CreateElementWithAttribute("filter", "type", FilterDescriptor.LoggerMatch.TypeNamespace).AppendTo(mAppender);

            mSut.Load(mAppender);

            Assert.AreEqual(2, mSut.ExistingFilters.Count);
            Assert.AreEqual(FilterDescriptor.DenyAll, mSut.ExistingFilters.First().Descriptor);
            Assert.AreEqual(FilterDescriptor.LoggerMatch, mSut.ExistingFilters.Skip(1).First().Descriptor);
        }

        [Test]
        public void Save_ShouldSaveEachExistingFilter()
        {
            mSut.AddFilter.Execute(FilterDescriptor.DenyAll);
            mSut.AddFilter.Execute(FilterDescriptor.DenyAll);

            XmlElement newAppender = mXmlDoc.CreateElement("appender");

            mSut.Save(mXmlDoc, newAppender);

            XmlNodeList appenderRefs = newAppender.SelectNodes($"filter[@type='{FilterDescriptor.DenyAll.TypeNamespace}']");

            Assert.IsNotNull(appenderRefs);
            Assert.AreEqual(2, appenderRefs.Count);
        }
    }
}
