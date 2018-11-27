// Copyright © 2018 Alex Leendertsen

using System;
using System.Windows.Input;
using System.Xml;
using Editor.Descriptors;
using Editor.Models.Base;
using Editor.Utilities;

namespace Editor.Models
{
    internal class ParamModel : NamedModel
    {
        private readonly Action<ParamModel> mShowFilterWindow;
        private readonly Action<ParamModel> mRemove;

        public ParamModel(XmlNode node,
                          Action<ParamModel> showFilterWindow = null,
                          Action<ParamModel> remove = null)
            : base(node, ParamDescriptor.Param)
        {
            mShowFilterWindow = showFilterWindow;
            mRemove = remove;

            Edit = new Command(EditFilterOnClick);
            Remove = new Command(RemoveFilterOnClick);
        }

        public string Value => Node.Attributes[Log4NetXmlConstants.Value]?.Value;

        public string Type => Node.Attributes[Log4NetXmlConstants.Type]?.Value;

        public override XmlNode Node
        {
            get => base.Node;
            set
            {
                base.Node = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Value));
                OnPropertyChanged(nameof(Type));
            }
        }

        public ICommand Edit { get; }

        public ICommand Remove { get; }

        private void EditFilterOnClick()
        {
            mShowFilterWindow(this);
        }

        private void RemoveFilterOnClick()
        {
            mRemove(this);
        }
    }
}
