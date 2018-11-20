using System;
using System.Diagnostics;
using System.Windows;

namespace Editor.Windows
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        private readonly Exception mException;

        public ErrorWindow(string source, Exception ex)
        {
            mException = ex;

            InitializeComponent();

            string githubLink = $"https://github.com/alexleen/log4net-config-editor/issues/new?title={source}: Unhandled Exception&body={ex.ToString()}&labels=bug,high priority";

            xGitHubHyperLink.NavigateUri = new Uri(githubLink);
        }

        private void CopyOnClick(object sender, RoutedEventArgs e)
        {
            //SetText throws for some reason - this does not
            Clipboard.SetDataObject(mException.ToString());
        }

        private void GitHubHyperLinkOnRequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
