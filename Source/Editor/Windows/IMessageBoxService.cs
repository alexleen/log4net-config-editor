// Copyright © 2018 Alex Leendertsen

namespace Editor.Windows
{
    public interface IMessageBoxService
    {
        /// <summary>
        /// Shows a Windows message box with an OK button and an Error icon along with the specified message.
        /// </summary>
        /// <param name="message"></param>
        void ShowError(string message);
    }
}
