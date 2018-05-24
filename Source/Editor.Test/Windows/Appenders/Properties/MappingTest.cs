// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using Editor.Windows.Appenders.Properties;
using Editor.Windows.PropertyCommon;
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
    }
}
