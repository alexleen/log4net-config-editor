// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors;
using Editor.Utilities;
using System;
using System.Windows.Input;
using System.Xml;

namespace Editor.Models
{
    internal class ParamModel : ChildModel
    {
        private readonly Action<ParamModel> mShowFilterWindow;
        private readonly Action<ParamModel> mRemove;

        public ParamModel(XmlNode node,
                          Action<ParamModel> showFilterWindow = null,
                          Action<ParamModel> remove = null)
            : base(ParamDescriptor.Param.ElementName, node)
        {
            mShowFilterWindow = showFilterWindow;
            mRemove = remove;

            Edit = new Command(EditFilterOnClick);
            Remove = new Command(RemoveFilterOnClick);
        }

        public string Name => Node.Attributes[Log4NetXmlConstants.Name]?.Value;

        public string Value => Node.Attributes[Log4NetXmlConstants.Value]?.Value;

        public string Type => Node.Attributes[Log4NetXmlConstants.Type]?.Value;

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
