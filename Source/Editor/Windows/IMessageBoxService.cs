// Copyright © 2018 Alex Leendertsen

using System.Windows;

namespace Editor.Windows
{
    public interface IMessageBoxService
    {
        /// <summary>
        /// Shows a Windows message box with an OK button and an Error icon along with the specified message.
        /// </summary>
        /// <param name="message"></param>
        void ShowError(string message);

        /// <summary>
        /// Shows a Windows message box with an OK button and an Information icon along with the specified message.
        /// </summary>
        /// <param name="message"></param>
        void ShowInformation(string message);

        /// <summary>
        /// Calls ShowDialog() on the specified window.
        /// </summary>
        /// <param name="window"></param>
        void ShowWindow(Window window);
    }
}
