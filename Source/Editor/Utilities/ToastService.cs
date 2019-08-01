using Editor.Interfaces;
using System;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace Editor.Utilities
{
    internal class ToastService : IToastService
    {
        private readonly Notifier mNotifier;

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
            });
        }

        public void ShowSuccess(string message)
        {
            mNotifier.ShowSuccess(message);
        }

        public void ShowInformation(string message)
        {
            mNotifier.ShowInformation(message);
        }

        public void ShowWarning(string message)
        {
            mNotifier.ShowWarning(message);
        }

        public void ShowError(string message)
        {
            mNotifier.ShowError(message);
        }
    }
}
