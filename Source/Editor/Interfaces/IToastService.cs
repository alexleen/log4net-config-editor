// Copyright © 2019 Alex Leendertsen

namespace Editor.Interfaces
{
    /// <summary>
    /// Provides access to a toast service that can show toast messages.
    /// </summary>
    public interface IToastService
    {
        /// <summary>
        /// Shows the specified messages in a red toast.
        /// </summary>
        /// <param name="message"></param>
        void ShowError(string message);

        /// <summary>
        /// Shows the specified messages in a blue toast.
        /// </summary>
        /// <param name="message"></param>
        void ShowInformation(string message);

        /// <summary>
        /// Shows the specified messages in a green toast.
        /// </summary>
        /// <param name="message"></param>
        void ShowSuccess(string message);

        /// <summary>
        /// Shows the specified messages in a yellow toast.
        /// </summary>
        /// <param name="message"></param>
        void ShowWarning(string message);
    }
}
