// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using Editor.Models;
using Editor.Windows.Appenders.Properties;
using Editor.Windows.PropertyCommon;
using log4net.Core;
using NUnit.Framework;

namespace Editor.Test.Windows.Appenders.Properties
{
    [TestFixture]
    public class MappingTest
    {
        private Mapping mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new Mapping(null, new ObservableCollection<IProperty>());
        }

        [Test]
        public void Load_ShouldLoadMappingCorrectly()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<appender>\r\n" +
                           "<mapping>\r\n" +
                           " <level value=\"ERROR\" />\r\n" +
                           " <foreColor value=\"DarkRed\" />\r\n" +
                           " <backColor value=\"White\" />\r\n" +
                           "</mapping>\r\n" +
                           "<mapping>\r\n" +
                           " <level value=\"WARN\" />\r\n" +
                           " <foreColor value=\"Yellow\" />\r\n" +
                           "</mapping>\r\n" +
                           "<mapping>\r\n" +
                           " <level value=\"info\" />\r\n" +
                           " <backColor value=\"White\" />\r\n" +
                           "</mapping>\r\n" +
                           "<mapping>\r\n" +
                           " <level value=\"\" />\r\n" +
                           " <foreColor value=\"Blue\" />\r\n" +
                           "</mapping>\r\n" +
                           "</appender>");

            IEnumerable<MappingModel> expectedMappings = new[]
            {
                new MappingModel(null, Level.Error, ConsoleColor.DarkRed, ConsoleColor.White, null, null),
                new MappingModel(null, Level.Warn, ConsoleColor.Yellow, null, null, null),
                new MappingModel(null, Level.Info, null, ConsoleColor.White, null, null)
            };

            mSut.Load(xmlDoc.FirstChild);

            CollectionAssert.AreEquivalent(expectedMappings, mSut.Mappings);
        }

        [Test]
        public void Save_ShouldSaveEachMappingCorrectly()
        {
            mSut.Mappings.Add(new MappingModel(null, Level.All, ConsoleColor.Black, ConsoleColor.Blue, null, null));
            mSut.Mappings.Add(new MappingModel(null, Level.Notice, ConsoleColor.Red, null, null, null));
            mSut.Mappings.Add(new MappingModel(null, Level.Emergency, null, ConsoleColor.Blue, null, null));

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement appender = xmlDoc.CreateElement("appender");

            mSut.Save(xmlDoc, appender);

            XmlNodeList mappings = appender.SelectNodes("/mapping");
            Assert.AreEqual(mSut.Mappings.Count, mappings.Count);

            foreach (XmlNode mappingNode in mappings)
            {
                string level = mappingNode["level"]?.Attributes["value"].Value;
                string foreColor = mappingNode["foreColor"]?.Attributes["value"].Value;
                string backColor = mappingNode["backColor"]?.Attributes["value"].Value;

                MappingModel match = mSut.Mappings.First(m => m.Level.Name == level);

                Assert.AreEqual(match.ForeColor?.ToString(), foreColor);
                Assert.AreEqual(match.BackColor?.ToString(), backColor);
            }
        }
    }
}
