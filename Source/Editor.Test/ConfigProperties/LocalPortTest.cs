using Editor.ConfigProperties;
using Editor.Interfaces;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class LocalPortTest : PortTestBase
    {
        internal override Port GetSut()
        {
            return new LocalPort(new ReadOnlyCollection<IProperty>(new List<IProperty>()));
        }

        [Test]
        public void Name_ShouldBeCorrect()
        {
            Assert.AreEqual("Local Port:", Sut.Name);
        }


    }
}
