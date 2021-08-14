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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Przypominajka_3._0.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class EventsMainView : UserControl
    {
        public EventsMainView()
        {
            InitializeComponent();
            boxik.Text = MainManager.testStr;
        }

        private void OnEventsViewLoaded(object sender, RoutedEventArgs e)
        {
            //LoadedEvent dupa = new LoadedEvent(0, "cupa", "506", 3, 1);
            //List<LoadedEvent> loadedEvents = new List<LoadedEvent>();
            //loadedEvents.Add(dupa);
            //loadedEvents.Add(dupa);
            //loadedEvents.Add(dupa);
            dataGrid1.ItemsSource = MainManager.loadedEvents;
            //MainManager.ChangeStatusInfo();
        }

        private void TEdt(object sender, MouseButtonEventArgs e)
        {
            LoadedEvent selected = (LoadedEvent)dataGrid1.SelectedItem;
            MessageBox.Show("DUPA "+selected.id);
        }
    }
}
