using Przypominajka_3._0.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Przypominajka_3._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //testStatus.Text = MainManager.DefaultStatusTextWorking;
            loadingGif.Visibility = Visibility.Visible;

            LoadData();
        }

        private async void LoadData()
        {
            await Task.Run(() =>
            {
                if(MainManager.SQL == null) MainManager.SQL = new SQLiteOperations();
                if (MainManager.SQL_LOTR == null) MainManager.SQL_LOTR = new SQLiteOperationsLOTR();
                MainManager.MainWindow = this;
                EventsManager.CheckEvents();
                LOTR_Manager.InitializeLOTRList();
            });
            DataContext = new EventsMainViewModel();
            //testStatus.Text = MainManager.DefaultStatusText;
            loadingGif.Visibility = Visibility.Hidden;
        }

        public void LoadEventsAddForEditing()
        {
            EventsAdd_Click(null,null);
        }

        public void LoadLOTRaddForEditing()
        {
            LOTRadd_Click(null, null);
        }

        public void LoadLOTRviewAfterEdit()
        {
            LOTRView_Click(null, null);
        }

        private void WindowIsLoaded(object sender, RoutedEventArgs e)
        {
            //DataContext = new EventsMainViewModel();
        }

        private void EventsView_Click(object sender, RoutedEventArgs e)
        {
            //testStatus.Text = "LOADING";
            DataContext = new EventsMainViewModel();
        }

        private void EventsAdd_Click(object sender, RoutedEventArgs e)
        {
            EventsManager.eventIsModified = sender == null;
            DataContext = new EventsAddViewModel();
        }
        private void LOTRView_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new LOTRViewModel();
        }
        private void LOTRadd_Click(object sender, RoutedEventArgs e)
        {
            LOTR_Manager.LoadedIssueIsModified = sender == null;
            DataContext = new LOTRAddViewModel();
        }

        private void autoStartChangeButton_Click(object sender, RoutedEventArgs e)
        {
            RegistryToggler.AutostartEditor();
        }

        private void goToResFolder_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(MainManager.DBdirPath);
        }

        private async void saveCopyOfDBs_Click(object sender, RoutedEventArgs e)
        {
            //testStatus.Text = MainManager.DefaultStatusTextWorking;
            loadingGif.Visibility = Visibility.Visible;
            await Task.Run(() =>
            {
                MainManager.CopyDBsToDesktop();
            });
            //testStatus.Text = MainManager.DefaultStatusText;
            loadingGif.Visibility = Visibility.Hidden;
        }

        private void createDesktopShortcut_Click(object sender, RoutedEventArgs e)
        {
            MainManager.CreateDesktopShortcut();
        }
    }
}
