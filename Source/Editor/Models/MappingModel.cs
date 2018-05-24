// Copyright © 2018 Alex Leendertsen

using System;
using System.Windows.Input;
using Editor.Utilities;
using log4net.Core;

namespace Editor.Models
{
    public class MappingModel : IEquatable<MappingModel>
    {
        private readonly Action<MappingModel> mRemove;

        public MappingModel(Level level, ConsoleColor? foreColor, ConsoleColor? backColor, Action<MappingModel> remove)
        {
            mRemove = remove;
            Level = level;
            ForeColor = foreColor;
            BackColor = backColor;
            Edit = new Command(EditFilterOnClick);
            Remove = new Command(RemoveFilterOnClick);
        }

        public Level Level { get; }

        public ConsoleColor? ForeColor { get; }

        public ConsoleColor? BackColor { get; }

        public ICommand Edit { get; }

        public ICommand Remove { get; }

        private void EditFilterOnClick()
        {
        }

        private void RemoveFilterOnClick()
        {
            mRemove(this);
        }

        public override string ToString()
        {
            return $"{Level} | {ForeColor} | {BackColor}";
        }

        public bool Equals(MappingModel other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(Level, other.Level) && ForeColor == other.ForeColor && BackColor == other.BackColor;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return Equals(obj as MappingModel);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Level != null ? Level.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ ForeColor.GetHashCode();
                hashCode = (hashCode * 397) ^ BackColor.GetHashCode();
                return hashCode;
            }
        }
    }
}
