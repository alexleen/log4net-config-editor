// Copyright © 2020 Alex Leendertsen

using System.Xml;
using Editor.Descriptors;
using Editor.Models;
using NUnit.Framework;

namespace Editor.Test.Models
{
    [TestFixture]
    public class FilterModelTest
    {
        private bool mFilterWindowShown;
        private bool mRemoveCalled;
        private bool mMoveUpCalled;
        private bool mMoveDownCalled;

        private FilterModel mSut;

        [SetUp]
        public void SetUp()
        {
            mFilterWindowShown = false;
            mRemoveCalled = false;
            mMoveUpCalled = false;
            mMoveDownCalled = false;

            mSut = new FilterModel(FilterDescriptor.LoggerMatch, null, ShowFilterWindow, Remove, MoveUp, MoveDown);
        }

        private void ShowFilterWindow(FilterModel filterModel)
        {
            mFilterWindowShown = true;
        }

        private void Remove(FilterModel filterModel)
        {
            mRemoveCalled = true;
        }

        private void MoveUp(FilterModel filterModel)
        {
            mMoveUpCalled = true;
        }

        private void MoveDown(FilterModel filterModel)
        {
            mMoveDownCalled = true;
        }

        [Test]
        public void AcceptOnMatch_ShouldReturnNull_WhenDenyAll()
        {
            mSut = new FilterModel(FilterDescriptor.DenyAll, null, ShowFilterWindow, Remove, MoveUp, MoveDown);
            
            Assert.IsNull(mSut.AcceptOnMatch);
        }

        [Test]
        public void AcceptOnMatch_ShouldReturnTrue_WhenNodeIsNull()
        {
            Assert.IsTrue(mSut.AcceptOnMatch);
        }

        [Test]
        public void AcceptOnMatch_ShouldReturnTrue_WhenAcceptOnMatchIsNotFound()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<filter type=\"log4net.Filter.DenyAllFilter\" />");

            mSut.Node = xmlDoc.FirstChild;

            Assert.IsTrue(mSut.AcceptOnMatch);
        }

        [Test]
        public void AcceptOnMatch_ShouldReturnTrue_WhenAcceptOnMatchCannotBeParsed()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<filter type=\"log4net.Filter.StringMatchFilter\">" +
                           "    <acceptOnMatch value=\"whatev\" />" +
                           "</filter>");

            mSut.Node = xmlDoc.FirstChild;

            Assert.IsTrue(mSut.AcceptOnMatch);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AcceptOnMatch_ShouldReturnAcceptOnMatch(bool accept)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<filter type=\"log4net.Filter.StringMatchFilter\">" +
                           $"    <acceptOnMatch value=\"{accept}\" />" +
                           "</filter>");

            mSut.Node = xmlDoc.FirstChild;

            Assert.AreEqual(accept, mSut.AcceptOnMatch);
        }

        [Test]
        public void Edit_Execute_ShouldShowFilterWindow()
        {
            mSut.Edit.Execute(null);

            AssertCalls(true, false, false, false);
        }

        [Test]
        public void Remove_Execute_ShouldCallRemove()
        {
            mSut.Remove.Execute(null);

            AssertCalls(false, true, false, false);
        }

        [Test]
        public void MoveUp_Execute_ShouldCallMoveUp()
        {
            mSut.MoveUp.Execute(null);

            AssertCalls(false, false, true, false);
        }

        [Test]
        public void MoveDown_Execute_ShouldCallMoveDown()
        {
            mSut.MoveDown.Execute(null);

            AssertCalls(false, false, false, true);
        }

        private void AssertCalls(bool showFilterWindow, bool remove, bool moveUp, bool moveDown)
        {
            Assert.AreEqual(showFilterWindow, mFilterWindowShown);
            Assert.AreEqual(remove, mRemoveCalled);
            Assert.AreEqual(moveUp, mMoveUpCalled);
            Assert.AreEqual(moveDown, mMoveDownCalled);
        }
    }
}
