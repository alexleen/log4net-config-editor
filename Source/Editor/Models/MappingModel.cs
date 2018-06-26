// Copyright © 2018 Alex Leendertsen

using Editor.Interfaces;
using Editor.Utilities;
using Editor.Windows.Appenders;
using log4net.Core;
using System;
using System.Windows.Input;
using System.Xml;

namespace Editor.Models
{
    public class MappingModel //: IEquatable<MappingModel>
    {
        private readonly Action<MappingModel> mRemove;
        private readonly Action<MappingModel> mShowMappingWindow;

        public MappingModel(XmlNode node,
                            Action<MappingModel> remove,
                            Action<MappingModel> showMappingWindow)
        {
            mRemove = remove;
            Node = node;
            mShowMappingWindow = showMappingWindow;
            Edit = new Command(EditFilterOnClick);
            Remove = new Command(RemoveFilterOnClick);
        }

        public ICommand Edit { get; }

        public ICommand Remove { get; }

        public XmlNode Node { get; set; }

        private void EditFilterOnClick()
        {
            mShowMappingWindow(this);
        }

        private void RemoveFilterOnClick()
        {
            mRemove(this);
        }

        //public override string ToString()
        //{
        //    return $"{Level} | {ForeColor} | {BackColor}";
        //}

        //public bool Equals(MappingModel other)
        //{
        //    if (other is null)
        //    {
        //        return false;
        //    }

        //    if (ReferenceEquals(this, other))
        //    {
        //        return true;
        //    }

        //    return Equals(Level, other.Level) && ForeColor == other.ForeColor && BackColor == other.BackColor;
        //}

        //public override bool Equals(object obj)
        //{
        //    if (obj is null)
        //    {
        //        return false;
        //    }

        //    if (ReferenceEquals(this, obj))
        //    {
        //        return true;
        //    }

        //    return Equals(obj as MappingModel);
        //}

        //public override int GetHashCode()
        //{
        //    unchecked
        //    {
        //        int hashCode = Level != null ? Level.GetHashCode() : 0;
        //        hashCode = (hashCode * 397) ^ ForeColor.GetHashCode();
        //        hashCode = (hashCode * 397) ^ BackColor.GetHashCode();
        //        return hashCode;
        //    }
        //}
    }
}
