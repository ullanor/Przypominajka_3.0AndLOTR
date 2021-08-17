using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Przypominajka_3._0
{
    public class SQLiteOperations
    {
        //MAINs
        #region MAIN
        SQLiteConnection sqlite_conn;
        public SQLiteOperations()
        {
            System.IO.Directory.CreateDirectory(MainManager.DBdirPath);
            System.IO.Directory.CreateDirectory(MainManager.imagesDir);
            if (!System.IO.File.Exists(MainManager.DBpath))
            {
                sqlite_conn = CreateConnection();
                CreateDataTable();
                sqlite_conn.Close();
            }
        }
        SQLiteConnection CreateConnection()
        {
            sqlite_conn = new SQLiteConnection($"Data Source={MainManager.DBpath}; Version = 3; New = True; Compress = True; ");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return sqlite_conn;
        }

        void CreateDataTable()
        {
            SQLiteCommand sqlite_cmd;
            //string Createsql = "DROP TABLE MTStatus";
            string Createsql = "CREATE TABLE IF NOT EXISTS PrzypominajkaEvents(" +
                "eventID INTEGER PRIMARY KEY," +
                "name TEXT," +
                "expirationDate TEXT," +
                "type INT NOT NULL," +
                "status INT NOT NULL" +
                ");";
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();
        }

        void ExecuteSQLCommand(string cmd)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText = cmd;
            sqlite_cmd.ExecuteNonQuery();
        }
        void ExecuteSQLCommandWithNameParameter(string cmd, string name)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText = cmd;
            sqlite_cmd.Parameters.AddWithValue("$name", name);
            sqlite_cmd.ExecuteNonQuery();
        }
        #endregion
        //EVENTS TABLE
        #region EVENTStable
        public void ModifyRowInTableEvents(string name, DateTime expDate, PrzypominajkaEventType type, int id)
        {
            sqlite_conn = CreateConnection();
            ExecuteSQLCommandWithNameParameter($"UPDATE PrzypominajkaEvents SET name=$name,expirationDate='{expDate.ToString("yyyy-MM-dd'T'hh:mm:ss.fff")}',type='{(int)type}' WHERE eventID='{id}';",name);
            sqlite_conn.Close();
        }
        public void ModifyRowInTableEvents(int id, bool markAsDone)
        {
            sqlite_conn = CreateConnection();
            ExecuteSQLCommand($"UPDATE PrzypominajkaEvents SET status='{(markAsDone? 1: 0)}' WHERE eventID='{id}';");
            sqlite_conn.Close();
        }
        public void DeleteRowInTableEvents(int id)
        {
            sqlite_conn = CreateConnection();
            ExecuteSQLCommand($"DELETE FROM PrzypominajkaEvents WHERE eventID='{id}';");
            sqlite_conn.Close();
        }

        public void InsertIntoTableEvents(string name, DateTime expDate, PrzypominajkaEventType type, bool status)
        {
            sqlite_conn = CreateConnection();
            ExecuteSQLCommandWithNameParameter($"INSERT INTO PrzypominajkaEvents(name, expirationDate, type, status) VALUES($name,'{expDate.ToString("yyyy-MM-dd'T'hh:mm:ss.fff")}','{(int)type}','{(status ? 1 : 0)}'); ",name);
            sqlite_conn.Close();
        }

        public void CheckDataFromTableEvents()
        {
            List<int> eventsToRemove = new List<int>();
            Dictionary<int,DateTime> eventsToMove1Week = new Dictionary<int, DateTime>();
            Dictionary<int, DateTime> eventsToMove2Weeks = new Dictionary<int, DateTime>();
            Dictionary<int, DateTime> eventsToMove1Month = new Dictionary<int, DateTime>();
            Dictionary<int, DateTime> eventsToMove1Year = new Dictionary<int, DateTime>();

            sqlite_conn = CreateConnection();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM PrzypominajkaEvents;";// where rowid=2 (counter for ID)
            sqlite_cmd.ExecuteNonQuery();

            sqlite_datareader = sqlite_cmd.ExecuteReader();

            DateTime expDate;
            DateTime curDate = DateTime.Now.Date;
            int TotalDaysLeft;
            PrzypominajkaEventType type;
            int status;
            int eventID;
            while (sqlite_datareader.Read())
            {
                expDate = Convert.ToDateTime(sqlite_datareader.GetString(2));
                TotalDaysLeft = (int)Math.Floor((expDate - curDate).TotalDays);              //TOTALDAYS!
                status = sqlite_datareader.GetInt32(4);
                if (TotalDaysLeft < 0 || status == 1)
                {
                    type = (PrzypominajkaEventType)sqlite_datareader.GetInt32(3);
                    if (type == PrzypominajkaEventType.Standard) { if (status == 0 || TotalDaysLeft >= 0) continue;/*if (TotalDaysLeft >= 0) continue;*/ }
                    eventID = sqlite_datareader.GetInt32(0);
                    switch (type)
                    {
                        case PrzypominajkaEventType.Annual:eventsToMove1Year.Add(eventID,expDate.AddYears(1));break;
                        case PrzypominajkaEventType.Monthly:eventsToMove1Month.Add(eventID,expDate.AddMonths(1));break;
                        case PrzypominajkaEventType.Fortnightly: eventsToMove2Weeks.Add(eventID,expDate.AddDays(14)); break;
                        case PrzypominajkaEventType.Weekly: eventsToMove1Week.Add(eventID,expDate.AddDays(7)); break;
                        default: eventsToRemove.Add(eventID); break;
                    }
                }
            }
            foreach(int id in eventsToRemove) ExecuteSQLCommand($"DELETE FROM PrzypominajkaEvents WHERE eventID='{id}';");
            foreach(KeyValuePair<int, DateTime> entry in eventsToMove1Year) ExecuteSQLCommand($"UPDATE PrzypominajkaEvents SET expirationDate='{entry.Value.ToString("yyyy-MM-dd'T'hh:mm:ss.fff")}',status={0} WHERE eventID='{entry.Key}';");
            foreach (KeyValuePair<int, DateTime> entry in eventsToMove1Month) ExecuteSQLCommand($"UPDATE PrzypominajkaEvents SET expirationDate='{entry.Value.ToString("yyyy-MM-dd'T'hh:mm:ss.fff")}',status={0} WHERE eventID='{entry.Key}';");
            foreach (KeyValuePair<int, DateTime> entry in eventsToMove2Weeks) ExecuteSQLCommand($"UPDATE PrzypominajkaEvents SET expirationDate='{entry.Value.ToString("yyyy-MM-dd'T'hh:mm:ss.fff")}',status={0} WHERE eventID='{entry.Key}';");
            foreach (KeyValuePair<int, DateTime> entry in eventsToMove1Week) ExecuteSQLCommand($"UPDATE PrzypominajkaEvents SET expirationDate='{entry.Value.ToString("yyyy-MM-dd'T'hh:mm:ss.fff")}',status={0} WHERE eventID='{entry.Key}';");
            sqlite_conn.Close();
        }

        public List<LoadedEvent> GetDataFromTableEvents()
        {
            List<LoadedEvent> loadedEvents = new List<LoadedEvent>();

            sqlite_conn = CreateConnection();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM PrzypominajkaEvents ORDER BY expirationDate ASC;";// where rowid=2 (counter for ID)
            sqlite_cmd.ExecuteNonQuery();

            sqlite_datareader = sqlite_cmd.ExecuteReader();

            DateTime expDate;
            DateTime curDate = DateTime.Now.Date;
            while (sqlite_datareader.Read())
            {
                expDate = Convert.ToDateTime(sqlite_datareader.GetString(2));
                loadedEvents.Add(new LoadedEvent()
                {
                    id = sqlite_datareader.GetInt32(0),
                    eName = sqlite_datareader.GetString(1),
                    eExp = expDate.ToString("dd'/'MM'/'yyyy"),
                    eType = (PrzypominajkaEventType)sqlite_datareader.GetInt32(3),
                    eStatus = sqlite_datareader.GetInt32(4),
                    eDAYS = (int)Math.Floor((expDate - curDate).TotalDays)         //TOTALDAYS!  
                });
            }
            sqlite_conn.Close();
            return loadedEvents;
        }
        #endregion
    }
}
