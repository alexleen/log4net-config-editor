// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Editor.Descriptors;
using Editor.Enums;
using NUnit.Framework;

namespace Editor.Test.Descriptors
{
    [TestFixture]
    internal class FilterDescriptorTest
    {
        private FieldInfo[] mFilters;

        [SetUp]
        public void SetUp()
        {
            mFilters = typeof(FilterDescriptor).GetFields(BindingFlags.Public | BindingFlags.Static);
        }

        private static readonly IEnumerable<TestCaseData> sFilterData = new[]
        {
            new TestCaseData("Deny All", FilterType.DenyAll, "log4net.Filter.DenyAllFilter", "filter"),
            new TestCaseData("Level", FilterType.LevelMatch, "log4net.Filter.LevelMatchFilter", "filter"),
            new TestCaseData("Level Range", FilterType.LevelRange, "log4net.Filter.LevelRangeFilter", "filter"),
            new TestCaseData("Logger", FilterType.LoggerMatch, "log4net.Filter.LoggerMatchFilter", "filter"),
            new TestCaseData("Property", FilterType.Property, "log4net.Filter.PropertyFilter", "filter"),
            new TestCaseData("String", FilterType.String, "log4net.Filter.StringMatchFilter", "filter")
        };

        [TestCaseSource(nameof(sFilterData))]
        public void EachFilter_ShouldHaveCorrectFilterField(string name, FilterType type, string typeNamespace, string elementName)
        {
            mFilters.Single(f => AreEqual((FilterDescriptor)f.GetValue(null), name, type, typeNamespace, elementName));
        }

        [Test]
        public void EachFilter_ShouldBeTested()
        {
            Assert.AreEqual(sFilterData.Count(), mFilters.Length);
        }

        [Test]
        public void EachFilter_ShouldHaveTestData()
        {
            foreach (FieldInfo info in mFilters)
            {
                sFilterData.Single(f => AreEqual((FilterDescriptor)info.GetValue(null), (string)f.Arguments[0], (FilterType)f.Arguments[1], (string)f.Arguments[2], (string)f.Arguments[3]));
            }
        }

        [TestCaseSource(nameof(sFilterData))]
        public void EachFilter_ShouldBeFoundByTypeNamespace(string name, FilterType type, string typeNamespace, string elementName)
        {
            Assert.IsTrue(FilterDescriptor.TryFindByTypeNamespace(typeNamespace, out FilterDescriptor filter));
            Assert.IsTrue(AreEqual(filter, name, type, typeNamespace, elementName));
        }

        private bool AreEqual(FilterDescriptor descriptor, string name, FilterType type, string typeNamespace, string elementName)
        {
            return descriptor.Name == name &&
                   descriptor.Type == type &&
                   descriptor.TypeNamespace == typeNamespace &&
                   descriptor.ElementName == elementName;
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("whatev")]
        public void TryFindByTypeNamespace_ShouldReturnFalse_AndNull_ForInvalidTypeNamespace(string typeNamespace)
        {
            Assert.IsFalse(FilterDescriptor.TryFindByTypeNamespace(typeNamespace, out FilterDescriptor descriptor));
            Assert.IsNull(descriptor);
        }
    }
}
