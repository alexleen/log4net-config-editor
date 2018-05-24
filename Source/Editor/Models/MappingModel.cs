// Copyright © 2018 Alex Leendertsen

using System;
using System.Windows.Input;
using Editor.Utilities;
using log4net.Core;

namespace Editor.Models
{
    public class MappingModel
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
    }
}
