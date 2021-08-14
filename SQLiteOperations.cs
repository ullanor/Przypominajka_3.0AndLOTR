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

        public void InsertIntoTableEvents(string name, DateTime expDate, PrzypominajkaEventType type, bool status)
        {
            sqlite_conn = CreateConnection();
            ExecuteSQLCommand($"INSERT INTO PrzypominajkaEvents(name, expirationDate, type, status) VALUES('{name}','{expDate.ToString("yyyy-MM-dd'T'hh:mm:ss.fff")}','{(int)type}','{(status ? 1 : 0)}'); ");
            sqlite_conn.Close();
        }

        public void InsertIntoTableLOTR(int issue,string imgSrc)
        {
            sqlite_conn = CreateConnection();
            ExecuteSQLCommand($"INSERT INTO PrzypominajkaLOTR(issue, imgSrc) VALUES('{issue}','{imgSrc}'); ");
            sqlite_conn.Close();
        }

        public List<LoadedEvent> GetDataFromTableEvents()
        {
            List<LoadedEvent> loadedEvents = new List<LoadedEvent>();
            sqlite_conn = CreateConnection();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM PrzypominajkaEvents;";// where rowid=2 (counter for ID)
            sqlite_cmd.ExecuteNonQuery();

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            short i = 1;
            while (sqlite_datareader.Read())
            {
                loadedEvents.Add(new LoadedEvent()
                {
                    id = i,
                    eName = sqlite_datareader.GetString(0),
                    eExp = Convert.ToDateTime(sqlite_datareader.GetString(1)),
                    eType = sqlite_datareader.GetInt32(2),
                    eStatus = sqlite_datareader.GetInt32(3)
                });
                i++;
            }
            return loadedEvents;
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
            return loadedLOTRs;
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

            for(int i = 1; i<= 91; i++)
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
    }
}
