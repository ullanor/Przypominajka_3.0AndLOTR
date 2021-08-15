using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Przypominajka_3._0
{
    public class SQLiteOperations
    {
        //MAINs
        #region MAIN
        SQLiteConnection sqlite_conn;
        public SQLiteOperations()
        {
            System.IO.Directory.CreateDirectory(SQLiteReferences.DBdirPath);
            System.IO.Directory.CreateDirectory(SQLiteReferences.imagesDir);
            if (!System.IO.File.Exists(SQLiteReferences.DBpath))
            {
                sqlite_conn = CreateConnection();
                CreateDataTable();
                sqlite_conn.Close();
            }
            //TEST!
            //MessageBox.Show("DONE: " + SQLiteReferences.DBpath);
            //InsertIntoTable("DUPA Test", DateTime.Now, PrzypominajkaEventType.Annual, false);
            //InsertIntoTable("DUP2A Test", DateTime.Now, PrzypominajkaEventType.Monthly, true);
            //InsertIntoTableEvents("DUP3A Test", DateTime.Now, PrzypominajkaEventType.Annual, false);
            //InsertIntoTableEvents("DUP4A Test", DateTime.Now, PrzypominajkaEventType.Annual, false);
            //InsertIntoTableLOTR(56, "");
        }
        SQLiteConnection CreateConnection()
        {
            sqlite_conn = new SQLiteConnection($"Data Source={SQLiteReferences.DBpath}; Version = 3; New = True; Compress = True; ");
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

            //LOTR
            Createsql = "CREATE TABLE IF NOT EXISTS PrzypominajkaLOTR(" +
                "issue INT NOT NULL UNIQUE," +
                "imgSrc TEXT" +
                ");";
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();

            for (int i = 1; i <= 91; i++)
            {
                ExecuteSQLCommand($"INSERT INTO PrzypominajkaLOTR(issue, imgSrc) VALUES('{i}','{i}'); ");
            }
        }

        void ExecuteSQLCommand(string cmd)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText = cmd;
            sqlite_cmd.ExecuteNonQuery();
        }
        #endregion
        //EVENTS TABLE
        #region EVENTStable
        public void ModifyRowInTableEvents(string name, DateTime expDate, PrzypominajkaEventType type, bool status, int id)
        {
            sqlite_conn = CreateConnection();
            ExecuteSQLCommand($"UPDATE PrzypominajkaEvents SET name='{name}',expirationDate='{expDate.ToString("yyyy-MM-dd'T'hh:mm:ss.fff")}',type='{(int)type}',status='{(status ? 1 : 0)}' WHERE eventID='{id}';");
            sqlite_conn.Close();
        }
        public void ModifyRowInTableEvents(int id)
        {
            sqlite_conn = CreateConnection();
            ExecuteSQLCommand($"UPDATE PrzypominajkaEvents SET status='{1}' WHERE eventID='{id}';");
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
            ExecuteSQLCommand($"INSERT INTO PrzypominajkaEvents(name, expirationDate, type, status) VALUES('{name}','{expDate.ToString("yyyy-MM-dd'T'hh:mm:ss.fff")}','{(int)type}','{(status ? 1 : 0)}'); ");
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
            DateTime curDate = DateTime.Now;
            int TotalDaysLeft;
            PrzypominajkaEventType type;
            int status;
            int eventID;
            while (sqlite_datareader.Read())
            {
                expDate = Convert.ToDateTime(sqlite_datareader.GetString(2));
                TotalDaysLeft = (expDate - curDate).Days;
                status = sqlite_datareader.GetInt32(4);
                if (TotalDaysLeft < 0 || status == 1)
                {
                    type = (PrzypominajkaEventType)sqlite_datareader.GetInt32(3);
                    if (type == PrzypominajkaEventType.Standard) { if (TotalDaysLeft >= 0) continue; }
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
            DateTime curDate = DateTime.Now;
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
                    eDAYS = (int)Math.Ceiling((expDate - curDate).TotalDays)             
                });
            }
            sqlite_conn.Close();
            return loadedEvents;
        }
        #endregion
        //LOTR table
        #region LOTRtable
        public void InsertIntoTableLOTR(int issue, string imgSrc)
        {
            sqlite_conn = CreateConnection();
            ExecuteSQLCommand($"INSERT INTO PrzypominajkaLOTR(issue, imgSrc) VALUES('{issue}','{imgSrc}'); ");
            sqlite_conn.Close();
        }

        public List<LoadedLOTR> GetDataFromTableLOTR()
        {
            List<LoadedLOTR> loadedLOTRs = new List<LoadedLOTR>();
            sqlite_conn = CreateConnection();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM PrzypominajkaLOTR;";// where rowid=2 (counter for ID)
            sqlite_cmd.ExecuteNonQuery();

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            short i = 1;
            while (sqlite_datareader.Read())
            {
                loadedLOTRs.Add(new LoadedLOTR()
                {
                    lIssue = sqlite_datareader.GetInt32(0),
                    limgSrc = sqlite_datareader.GetString(1)
                });
                i++;
            }
            sqlite_conn.Close();
            return loadedLOTRs;
        }
        #endregion
    }
}
