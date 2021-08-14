using Przypominajka_3._0.ViewModels;
using System;
using System.Collections.Generic;
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
            MainManager.SQL = new SQLiteOperations();
            MainManager.testStr = "HEJKA FROM WINGOD";

            MainManager.InitializeEventsList();
            LOTR_Manager.InitializeLOTRList();
            testStatus.Text = MainManager.DefaultStatusText;
            MainManager.MainWindow = this;
        }

        private void WindowIsLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void EventsView_Click(object sender, RoutedEventArgs e)
        {
            testStatus.Text = "LOADING";
            DataContext = new EventsMainViewModel();
        }

        private void EventsAdd_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new EventsAddViewModel();
        }
        private void LOTRView_Click(object sender, RoutedEventArgs e)
        {
            testStatus.Text = "LOADING";
            DataContext = new LOTRViewModel();
        }
        private void LOTRadd_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new LOTRAddViewModel();
        }

    }
}
