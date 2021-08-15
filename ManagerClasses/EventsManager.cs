using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Przypominajka_3._0
{
    public static class EventsManager
    {
        public static void InitializeEventsList()
        {
            loadedEvents = MainManager.SQL.GetDataFromTableEvents();
            SelectedEvent = null;
        }
        public static List<LoadedEvent> loadedEvents;
        public static LoadedEvent SelectedEvent;

        public static List<LoadedEvent> MarkSelectedEvent()
        {
            MainManager.SQL.ModifyRowInTableEvents(SelectedEvent.id);
            InitializeEventsList();
            return loadedEvents;
        }
        public static List<LoadedEvent> DeleteSelectedEvent()
        {
            MainManager.SQL.DeleteRowInTableEvents(SelectedEvent.id);
            InitializeEventsList();
            return loadedEvents;
        }

        public static void CreateNewEvent(string name, DateTime date, PrzypominajkaEventType type)
        {
            MainManager.SQL.InsertIntoTableEvents(name, date, type, false);
            InitializeEventsList();
        }

        public static List<LoadedEvent> CheckEvents()
        {
            MainManager.SQL.CheckDataFromTableEvents();
            InitializeEventsList();
            return loadedEvents;
        }
    }

    public class LoadedEvent
    {
        public int id { get; set; }
        public string eName { get; set; }
        public String eExp { get; set; }
        public PrzypominajkaEventType eType { get; set; }
        public int eStatus { get; set; }

        public int eDAYS { get; set; }//total days until expiration and date
    }
}
