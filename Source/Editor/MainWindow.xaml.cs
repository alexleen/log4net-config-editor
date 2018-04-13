using System.IO;
using System.Windows;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository;
using Microsoft.Win32;

namespace Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            xChildren.ItemsSource = new[] { new ConsoleAppender { Name = "console-test" } };
        }

        private void OpenOnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Config Files (*.xml, *.config) | *.xml; *.config" };

            bool? showDialog = ofd.ShowDialog(this);

            if (showDialog.Value)
            {
                XmlConfigurator.Configure(new FileInfo(ofd.FileName));

                ILoggerRepository loggerRepository = LogManager.GetRepository();
                IAppender[] appenders = loggerRepository.GetAppenders();
            }
        }
    }
}