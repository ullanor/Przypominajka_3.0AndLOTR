using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Przypominajka_3._0
{
    public static class MainManager
    {
        public static SQLiteOperations SQL;
        public static string testStr;
        public static string DefaultStatusText = "OK";
        public static MainWindow MainWindow;

        public static void ChangeStatusInfo()
        {
            MainWindow.testStatus.Text = DefaultStatusText;
        }
        public static void InitializeEventsList()
        {
            loadedEvents = SQL.GetDataFromTableEvents();
        }
        public static List<LoadedEvent> loadedEvents;

    }

    public class LoadedEvent
    {
        public int id { get; set; }
        public string eName { get; set; }
        public DateTime eExp { get; set; }
        public int eType { get; set; }
        public int eStatus { get; set; }
    }
}
