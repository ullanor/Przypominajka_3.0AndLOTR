using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Przypominajka_3._0
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool LaunchedViaStartup { get; set; }

        void App_Startup(object sender, StartupEventArgs e)
        {
            string[] args = e.Args;
            LaunchedViaStartup = args != null && args.Any(arg => arg.Equals("startup", StringComparison.CurrentCultureIgnoreCase));

            if (LaunchedViaStartup == true)
            {
                //MessageBox.Show("przez startup shortcata");
                StartupMainWindow startWindow = new StartupMainWindow();
                startWindow.Show();
                //var application = new App();
                //application.StartupUri = new System.Uri("MainWindow.xaml", System.UriKind.Relative);
                //application.InitializeComponent();
                //application.Run();
            }
            else
            {
                //MessageBox.Show("normalnie");
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                //var application = new App();
                //application.StartupUri = new System.Uri("BasicMainWindow.xaml", System.UriKind.Relative);
                //application.InitializeComponent();
                //application.Run();
            }

            // Create main application window, starting minimized if specified
            //MainWindow mainWindow = new MainWindow();
            //if (startMinimized)
            //{
            //    mainWindow.WindowState = WindowState.Minimized;
            //}
            //mainWindow.Show();
        }
    }
}
