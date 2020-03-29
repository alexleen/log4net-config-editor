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
                           XmlNode node,
                           Action<FilterModel> showFilterWindow,
                           Action<FilterModel> remove,
                           Action<FilterModel> moveUp,
                           Action<FilterModel> moveDown)
            : base(node, descriptor)
        {
            mShowFilterWindow = showFilterWindow;
            mRemove = remove;
            mMoveUp = moveUp;
            mMoveDown = moveDown;

            Edit = new Command(EditFilterOnClick);
            Remove = new Command(RemoveFilterOnClick);
            MoveUp = new Command(MoveFilterUpOnClick);
            MoveDown = new Command(MoveFilterDownOnClick);
        }

        /// <summary>
        /// The value of the Accept On Match property of a filter.
        /// Null for the Deny All filter.
        /// </summary>
        public bool? AcceptOnMatch
        {
            get
            {
                if (Descriptor == FilterDescriptor.DenyAll)
                {
                    //AcceptOnMatch doesn't apply to the Deny All filter
                    return null;
                }

                if (Node != null && bool.TryParse(Node.GetValueAttributeValueFromChildElement("acceptOnMatch"), out bool acceptOnMatch))
                {
                    return acceptOnMatch;
                }

                //1) If there's no node, it means we're creating a new filter. Accept On Match is true by default.
                //2) AcceptOnMatch cannot be found. Accept On Match is true by default.
                //3) AcceptOnMatch cannot be parsed. True seems appropriate (?)
                return true;
            }
        }

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
