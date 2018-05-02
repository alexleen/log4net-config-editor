// Copyright © 2018 Alex Leendertsen

using System;
using System.Windows.Input;

namespace Editor.Utilities
{
    public class Command : ICommand
    {
        private readonly Action mExecute;
        private readonly Func<bool> mCanExecute;

        public Command(Action execute, Func<bool> canExecute = null)
        {
            mExecute = execute;
            mCanExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return mCanExecute == null || mCanExecute.Invoke();
        }

        public void Execute(object parameter)
        {
            mExecute();
        }

        public event EventHandler CanExecuteChanged;
    }
}
