// Copyright © 2018 Alex Leendertsen

using System;
using System.Windows;
using Editor.Interfaces;
using Microsoft.Win32;

namespace Editor.Utilities
{
    internal class MessageBoxService : IMessageBoxService
    {
        private readonly Window mOwner;

        public MessageBoxService(Window owner)
        {
            mOwner = owner ?? throw new ArgumentNullException(nameof(owner));
        }

        public void ShowError(string message)
        {
            MessageBox.Show(mOwner, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowWarning(string message)
        {
            MessageBox.Show(mOwner, message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void ShowInformation(string message)
        {
            MessageBox.Show(mOwner, message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowWindow(Window window)
        {
            window.Owner = mOwner;
            window.ShowDialog();
        }

        public bool? ShowOpenFileDialog(out string fileName)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            bool? showDialog = ofd.ShowDialog();

            if (showDialog.IsTrue())
            {
                fileName = ofd.FileName;
                return true;
            }

            fileName = null;
            return showDialog;
        }
    }
}
