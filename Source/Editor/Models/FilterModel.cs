// Copyright © 2018 Alex Leendertsen

using System;
using System.Windows.Input;
using System.Xml;
using Editor.Descriptors;
using Editor.Models.Base;
using Editor.Utilities;

namespace Editor.Models
{
    public class FilterModel : ModelBase
    {
        private readonly Action<FilterModel> mShowFilterWindow;
        private readonly Action<FilterModel> mRemove;
        private readonly Action<FilterModel> mMoveUp;
        private readonly Action<FilterModel> mMoveDown;

        public FilterModel(FilterDescriptor descriptor,
                           XmlNode node,
                           Action<FilterModel> showFilterWindow,
                           Action<FilterModel> remove,
                           Action<FilterModel> moveUp,
                           Action<FilterModel> moveDown)
        {
            mShowFilterWindow = showFilterWindow;
            mRemove = remove;
            mMoveUp = moveUp;
            mMoveDown = moveDown;

            Descriptor = descriptor;
            Node = node;

            Edit = new Command(EditFilterOnClick);
            Remove = new Command(RemoveFilterOnClick);
            MoveUp = new Command(MoveFilterUpOnClick);
            MoveDown = new Command(MoveFilterDownOnClick);
        }

        public FilterDescriptor Descriptor { get; }

        public ICommand Edit { get; }

        public ICommand Remove { get; }

        public ICommand MoveUp { get; }

        public ICommand MoveDown { get; }

        private void EditFilterOnClick()
        {
            mShowFilterWindow(this);
        }

        private void RemoveFilterOnClick()
        {
            mRemove(this);
        }

        private void MoveFilterUpOnClick()
        {
            mMoveUp(this);
        }

        private void MoveFilterDownOnClick()
        {
            mMoveDown(this);
        }
    }
}
