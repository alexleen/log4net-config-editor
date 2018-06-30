// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Xml;
using Editor.Models;
using Editor.Test.TestUtilities;
using log4net.Core;
using NUnit.Framework;

namespace Editor.Test.Models
{
    [TestFixture]
    public class MappingModelTest : EqualityTests<MappingModel>
    {
        private static readonly Level sLevel = Level.All;
        private const ConsoleColor ForeColor = ConsoleColor.Black;
        private const ConsoleColor BackColor = ConsoleColor.Blue;
        private XmlDocument mXmlDoc;

        public override void SetUp()
        {
            mXmlDoc = new XmlDocument();
            mXmlDoc.LoadXml($"<mapping><level value = \"{sLevel.Name}\" /><foreColor value = \"{ForeColor}\" /><backColor value = \"{BackColor}\" /></mapping> ");
            base.SetUp();
        }

        [Test]
        public void ToString_ShouldReturnCorrectString()
        {
            Assert.AreEqual($"{sLevel} | {ForeColor} | {BackColor}", Sut.ToString());
        }

        protected override MappingModel GetSut()
        {
            return new MappingModel(Dummy, Dummy, mXmlDoc.FirstChild);
        }

        protected override MappingModel GetOtherEqual()
        {
            return new MappingModel(Dummy, Dummy, mXmlDoc.FirstChild);
        }

        protected override IEnumerable<MappingModel> GetOthersNotEqual()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml($"<mapping><level value = \"ALERT\" /><foreColor value = \"Blue\" /><backColor value = \"Red\" /></mapping> ");

            yield return new MappingModel(Dummy, Dummy, xmlDoc.FirstChild);

            xmlDoc.LoadXml($"<mapping><level value = \"ALERT\" /><foreColor value = \"{ForeColor}\" /><backColor value = \"{BackColor}\" /></mapping> ");

            yield return new MappingModel(Dummy, Dummy, xmlDoc.FirstChild);

            xmlDoc.LoadXml($"<mapping><level value = \"{sLevel.Name}\" /><foreColor value = \"Blue\" /><backColor value = \"{BackColor}\" /></mapping> ");

            yield return new MappingModel(Dummy, Dummy, xmlDoc.FirstChild);

            xmlDoc.LoadXml($"<mapping><level value = \"{sLevel.Name}\" /><foreColor value = \"{ForeColor}\" /><backColor value = \"Red\" /></mapping> ");

            yield return new MappingModel(Dummy, Dummy, xmlDoc.FirstChild);
        }

        protected override int ExpectedHashCode
        {
            get
            {
                int hashCode = sLevel.GetHashCode();
                hashCode = (hashCode * 397) ^ ForeColor.GetHashCode();
                hashCode = (hashCode * 397) ^ BackColor.GetHashCode();
                return hashCode;
            }
        }

        private void Dummy(MappingModel mappingModel)
        {
        }
    }
}
