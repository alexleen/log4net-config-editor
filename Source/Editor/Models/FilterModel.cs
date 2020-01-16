// Copyright © 2020 Alex Leendertsen

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
                           bool acceptOnMatch,
                           XmlNode node,
                           Action<FilterModel> showFilterWindow,
                           Action<FilterModel> remove,
                           Action<FilterModel> moveUp,
                           Action<FilterModel> moveDown)
            : base(node, descriptor)
        {
            AcceptOnMatch = acceptOnMatch;

            mShowFilterWindow = showFilterWindow;
            mRemove = remove;
            mMoveUp = moveUp;
            mMoveDown = moveDown;

            Edit = new Command(EditFilterOnClick);
            Remove = new Command(RemoveFilterOnClick);
            MoveUp = new Command(MoveFilterUpOnClick);
            MoveDown = new Command(MoveFilterDownOnClick);
        }

        public bool AcceptOnMatch { get; }

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
