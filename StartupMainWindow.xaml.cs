using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Przypominajka_3._0
{
    /// <summary>
    /// Interaction logic for StartupMainWindow.xaml
    /// </summary>
    public partial class StartupMainWindow : Window
    {
        public StartupMainWindow()
        {
            InitializeComponent();
            eventsInfo.Text = "loading";
            Left = System.Windows.SystemParameters.WorkArea.Width - Width;
            Top = System.Windows.SystemParameters.WorkArea.Height - Height;
            LoadData();
        }

        private async void LoadData()
        {
            await Task.Run(() =>
            {
                if (MainManager.SQL == null) MainManager.SQL = new SQLiteOperations();
                SetInfoString(EventsManager.CheckEvents());
            });
            //SetInfoString(source);
            //eventsInfo.Text = source.Count().ToString();
        }

        private void SetInfoString(List<LoadedEvent> events)
        {
            int lessThan3DaysCount = 0;
            int todayCount = 0;
            foreach(LoadedEvent evento in events)
            {
                if (evento.eDAYS == 0) { todayCount++; continue; }
                if (evento.eDAYS < 3) { lessThan3DaysCount++; continue; }
            }
            Thread.Sleep(2000); //TEStING!!!
            Dispatcher.Invoke(() =>
            {
                eventsInfo.Text = $"Total Events: {events.Count()} \n{lessThan3DaysCount} -> will expire in less than 3 days!\n" +
                    $"{todayCount} -> will expire today!";
            });
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MoveToMain_Click(object sender, RoutedEventArgs e)
        {

            MainWindow MainWindowOfP = new MainWindow();
            MainWindowOfP.Show();
            Close();
        }
    }
}
