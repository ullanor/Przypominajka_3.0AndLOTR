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
        }

        private void OnEventsViewLoaded(object sender, RoutedEventArgs e)
        {
            EventsManager.SelectedEvent = null;
            dataGridEvents.ItemsSource = EventsManager.loadedEvents;
            EventInfo.Text = EventsManager.loadedEvents.Count().ToString();
        }

        private void OnEventSelected(object sender, SelectionChangedEventArgs e)
        {
            EventsManager.SelectedEvent = (LoadedEvent)dataGridEvents.SelectedItem;
            SetEventsControlsVisibility();
        }

        private void SetEventsControlsVisibility()
        {
            bool visible = EventsManager.SelectedEvent != null;

            EditEventButton.IsEnabled = visible;
            MarkEventButton.IsEnabled = visible;
            RemoveEventButton.IsEnabled = visible;
            UnMarkEventButton.IsEnabled = visible;    
        }

        private async void MarkEventButton_Click(object sender, RoutedEventArgs e)
        {
            bool markAsDone = (sender as Button).Name == MarkEventButton.Name;
            MainManager.ChangeStatusInfo(false);
            var source = await Task.Run(() =>
            {
                return EventsManager.MarkSelectedEvent(markAsDone);
            });
            dataGridEvents.ItemsSource = source;
            SetEventsControlsVisibility();
            MainManager.ChangeStatusInfo(true);
        }

        private async void RemoveEventButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult != MessageBoxResult.Yes) return;

            MainManager.ChangeStatusInfo(false);
            var source = await Task.Run(() =>
            {
                return EventsManager.DeleteSelectedEvent();
            });
            dataGridEvents.ItemsSource = source;
            SetEventsControlsVisibility();
            MainManager.ChangeStatusInfo(true);
            EventInfo.Text = EventsManager.loadedEvents.Count().ToString();
        }

        private void EditEventButton_Click(object sender, RoutedEventArgs e)
        {
            MainManager.MainWindow.LoadEventsAddForEditing();
        }

        private async void CheckDataButton_Click(object sender, RoutedEventArgs e)
        {
            MainManager.ChangeStatusInfo(false);
            var source = await Task.Run(() =>
            {
                return EventsManager.CheckEvents();
            });
            dataGridEvents.ItemsSource = source;
            SetEventsControlsVisibility();
            MainManager.ChangeStatusInfo(true);
            EventInfo.Text = EventsManager.loadedEvents.Count().ToString();
        }
    }
}
