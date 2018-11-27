// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors;
using Editor.Models;
using NUnit.Framework;

namespace Editor.Test.Models
{
    [TestFixture]
    public class FilterModelTest
    {
        private bool mFiterWindowShown;
        private bool mRemoveCalled;
        private bool mMoveUpCalled;
        private bool mMoveDownCalled;

        private FilterModel mSut;

        [SetUp]
        public void SetUp()
        {
            mFiterWindowShown = false;
            mRemoveCalled = false;
            mMoveUpCalled = false;
            mMoveDownCalled = false;

            mSut = new FilterModel(FilterDescriptor.DenyAll, null, ShowFilterWindow, Remove, MoveUp, MoveDown);
        }

        private void ShowFilterWindow(FilterModel filterModel)
        {
            mFiterWindowShown = true;
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
            Assert.AreEqual(showFilterWindow, mFiterWindowShown);
            Assert.AreEqual(remove, mRemoveCalled);
            Assert.AreEqual(moveUp, mMoveUpCalled);
            Assert.AreEqual(moveDown, mMoveDownCalled);
        }
    }
}
