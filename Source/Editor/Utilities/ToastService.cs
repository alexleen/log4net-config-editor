// Copyright © 2019 Alex Leendertsen

using System;
using System.Windows;
using Editor.Interfaces;
using ToastNotifications;
using ToastNotifications.Core;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace Editor.Utilities
{
    internal class ToastService : IToastService
    {
        private readonly Notifier mNotifier;
        private readonly MessageOptions mMessageOptions;

        public ToastService()
        {
            mNotifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: Application.Current.MainWindow,
                    corner: Corner.BottomCenter,
                    offsetX: 0,
                    offsetY: 60);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(5),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                cfg.Dispatcher = Application.Current.Dispatcher;

                cfg.DisplayOptions.TopMost = false;
                cfg.DisplayOptions.Width = 300;
            });

            mMessageOptions = new MessageOptions { UnfreezeOnMouseLeave = true };
        }

        public void ShowSuccess(string message)
        {
            mNotifier.ShowSuccess(message, mMessageOptions);
        }

        public void ShowInformation(string message)
        {
            mNotifier.ShowInformation(message, mMessageOptions);
        }

        public void ShowWarning(string message)
        {
            mNotifier.ShowWarning(message, mMessageOptions);
        }

        public void ShowError(string message)
        {
            mNotifier.ShowError(message, mMessageOptions);
        }
    }
}
