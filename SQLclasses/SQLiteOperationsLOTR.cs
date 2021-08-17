using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Przypominajka_3._0
{
    public class SQLiteOperationsLOTR
    {
        SQLiteConnection sqlite_conn;
        public SQLiteOperationsLOTR()
        {
            if (!System.IO.File.Exists(MainManager.DBpathLOTR))
            {
                MainManager.ExtractFileResource("Przypominajka_3._0.Assets.oneRingSmall.png", MainManager.standardImage);
                sqlite_conn = CreateConnectionLOTR(); 
                CreateDataTableLOTR();
                sqlite_conn.Close();
            }
        }

        SQLiteConnection CreateConnectionLOTR()
        {
            sqlite_conn = new SQLiteConnection($"Data Source={MainManager.DBpathLOTR}; Version = 3; New = True; Compress = True; ");
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

        void CreateDataTableLOTR()
        {
            SQLiteCommand sqlite_cmd;
            //LOTR
            string Createsql = "CREATE TABLE IF NOT EXISTS LOTRdeagostiniIssues(" +
                "issue INT NOT NULL UNIQUE," +
                "imgSrc TEXT," +
                "guide TEXT," +
                "play TEXT," +
                "battle TEXT," +
                "paint TEXT," +
                "model TEXT," +
                "extras TEXT" +
                ");";
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();
            for (int i = 1; i <= 91; i++)
            {
                ExecuteSQLCommand($"INSERT INTO LOTRdeagostiniIssues(issue, imgSrc, guide, play, battle, paint, model, extras) VALUES('{i}','{MainManager.standardImage}','','','','','','');");
            }
        }

        //public void InsertIntoTableLOTR(int issue, string imgSrc)
        //{
        //    sqlite_conn = CreateConnectionLOTR();
        //    ExecuteSQLCommand($"INSERT INTO LOTRdeagostiniIssues(issue, imgSrc) VALUES('{issue}','{imgSrc}'); ");
        //    sqlite_conn.Close();
        //}

        public void ModifyTableLOTR(int issue, string imgSrc, string[] desc6)
        {
            sqlite_conn = CreateConnectionLOTR();
            //ExecuteSQLCommand($"UPDATE LOTRdeagostiniIssues SET imgSrc='{imgSrc}'," +
            //    $"guide='{desc6[0]}',play='{desc6[1]}',battle='{desc6[2]}',paint='{desc6[3]}',model='{desc6[4]}',extras='{desc6[5]}'  WHERE issue='{issue}'; ");

            ExecuteSQLCommandWithNameParameters($"UPDATE LOTRdeagostiniIssues SET imgSrc='{imgSrc}'," +
                    $"guide=$guide,play=$play,battle=$battle,paint=$paint,model=$model,extras=$extras  WHERE issue='{issue}'; ",desc6);
            sqlite_conn.Close();
        }

        public List<LoadedLOTR> GetDataFromTableLOTR()
        {
            List<LoadedLOTR> loadedLOTRs = new List<LoadedLOTR>();
            sqlite_conn = CreateConnectionLOTR();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM LOTRdeagostiniIssues;";// where rowid=2 (counter for ID)
            sqlite_cmd.ExecuteNonQuery();

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            short i = 1;
            while (sqlite_datareader.Read())
            {
                loadedLOTRs.Add(new LoadedLOTR()
                {
                    lIssue = sqlite_datareader.GetInt32(0),
                    limgSrc = sqlite_datareader.GetString(1),
                    lguide = sqlite_datareader.GetString(2),
                    lplay = sqlite_datareader.GetString(3),
                    lbattle = sqlite_datareader.GetString(4),
                    lpaint = sqlite_datareader.GetString(5),
                    lmodel = sqlite_datareader.GetString(6),
                    lextras = sqlite_datareader.GetString(7)
                });
                i++;
            }
            sqlite_conn.Close();
            return loadedLOTRs;
        }

        void ExecuteSQLCommand(string cmd)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText = cmd;
            sqlite_cmd.ExecuteNonQuery();
        }
        void ExecuteSQLCommandWithNameParameters(string cmd, string[] desc6)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText = cmd;
            sqlite_cmd.Parameters.AddWithValue("$guide", desc6[0]);
            sqlite_cmd.Parameters.AddWithValue("$play", desc6[1]);
            sqlite_cmd.Parameters.AddWithValue("$battle", desc6[2]);
            sqlite_cmd.Parameters.AddWithValue("$paint", desc6[3]);
            sqlite_cmd.Parameters.AddWithValue("$model", desc6[4]);
            sqlite_cmd.Parameters.AddWithValue("$extras", desc6[5]);
            sqlite_cmd.ExecuteNonQuery();
        }
    }
}
