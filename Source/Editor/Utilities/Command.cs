// Copyright © 2018 Alex Leendertsen

using System;
using System.Windows.Input;

namespace Editor.Utilities
{
    public class Command : ICommand
    {
        private readonly Action<object> mExecuteWithParam;
        private readonly Action mExecute;
        private readonly Func<bool> mCanExecute;

        public Command(Action execute, Func<bool> canExecute = null)
        {
            mExecute = execute;
            mCanExecute = canExecute;
        }

        public Command(Action<object> executeWithParam)
        {
            mExecuteWithParam = executeWithParam;
        }

        public bool CanExecute(object parameter)
        {
            return mCanExecute == null || mCanExecute.Invoke();
        }

        public void Execute(object parameter)
        {
            if (mExecuteWithParam != null)
            {
                mExecuteWithParam(parameter);
            }
            else
            {
                mExecute?.Invoke();
            }
        }

#pragma warning disable 67
        public event EventHandler CanExecuteChanged;
#pragma warning restore 67
    }
}
