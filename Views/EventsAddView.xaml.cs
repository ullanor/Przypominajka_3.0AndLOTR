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

namespace Przypominajka_3._0.Views
{
    /// <summary>
    /// Interaction logic for SecondView.xaml
    /// </summary>
    public partial class EventsAddView : UserControl
    {
        private string ButtonSave = "Save Event";
        private string ButtonModify = "Modify Event";
        private bool eventIsModified;
        private LoadedEvent evento;
        public EventsAddView()
        {
            InitializeComponent();
            if (EventsManager.eventIsModified) { SetEditedEventData(); eventIsModified = true; }
        }

        private void SetEditedEventData()
        {
            evento = EventsManager.SelectedEvent;
            EventExpTime.SelectedDate = Convert.ToDateTime(evento.eExp);
            EventType.SelectedItem = EventType.Items[(int)evento.eType];
            EventName.Text = evento.eName;
            SaveEventButton.Content = ButtonModify;
        }
        private void ClearEventFields()
        {
            EventExpTime.SelectedDate = null;
            EventType.SelectedItem = null;
            EventName.Text = string.Empty;
            SaveEventButton.Content = ButtonSave;
        }
        private void SaveEvent_Click(object sender, RoutedEventArgs e)
        {
            if (EventExpTime.SelectedDate != null && EventType.SelectedItem != null)
            {
                DateTime dt;
                if (!EventExpTime.SelectedDate.HasValue) dt = DateTime.Now.Date;
                else dt = ((DateTime)EventExpTime.SelectedDate).Date;
                if (!eventIsModified) EventsManager.CreateNewEvent(EventName.Text, dt, (PrzypominajkaEventType)EventType.SelectedItem);
                else EventsManager.ModifyEvent(EventName.Text, dt, (PrzypominajkaEventType)EventType.SelectedItem,evento.id);
                ClearEventFields();
            }
        }
    }
}
