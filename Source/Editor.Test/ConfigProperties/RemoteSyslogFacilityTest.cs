// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Editor.ConfigProperties;
using Editor.Interfaces;
using NUnit.Framework;
using static log4net.Appender.RemoteSyslogAppender;

namespace Editor.Test.ConfigProperties
{
    [TestFixture]
    public class RemoteSyslogFacilityTest
    {
        private RemoteSyslogFacility mSut;

        [SetUp]
        public void SetUp()
        {
            mSut = new RemoteSyslogFacility(new ReadOnlyCollection<IProperty>(new List<IProperty>()));
        }

        [Test]
        public void RowHeight_ShouldBeCorrect()
        {
            Assert.AreEqual(mSut.RowHeight, GridLength.Auto);
        }

        [Test]
        public void Name_ShouldBeCorrect()
        {
            Assert.AreEqual("Facility:", mSut.Name);
        }

        [Test]
        public void Width_ShouldBeCorrect()
        {
            Assert.AreEqual(110, mSut.Width);
        }

        [Test]
        public void SelectedValue_ShouldBeInitializedToUser()
        {
            Assert.AreEqual(SyslogFacility.User.ToString(), mSut.SelectedValue);
        }
    }
}
