// Copyright © 2018 Alex Leendertsen

using System;
using System.Threading.Tasks;
using System.Windows;
using Editor.Windows;

namespace Editor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SetupExceptionHandling();
        }

        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) => OnUnhandledException("AppDomain", (Exception)e.ExceptionObject);

            DispatcherUnhandledException += (s, e) => OnUnhandledException("Dispatcher", e.Exception);

            TaskScheduler.UnobservedTaskException += (s, e) => OnUnhandledException("TaskScheduler", e.Exception);
        }

        private void OnUnhandledException(string source, Exception ex)
        {
            ErrorWindow errorWindow = new ErrorWindow(source, ex);

            errorWindow.ShowDialog();
        }
    }
}
