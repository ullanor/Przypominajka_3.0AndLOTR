using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Przypominajka_3._0
{
    public static class MainManager
    {
        public static SQLiteOperations SQL;
        public static SQLiteOperationsLOTR SQL_LOTR;
        public static string testStr;
        public static string DefaultStatusText = "Ready";
        public static string DefaultStatusTextWorking = "Working";
        public static MainWindow MainWindow;

        public static string DBdirPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\PrzypominajkaDirectory";
        public static string DBpath = DBdirPath + "\\PrzypominajkaDB.db";
        public static string DBpathLOTR = DBdirPath + "\\LOTRdeagostiniDB.db";
        public static string imagesDir = DBdirPath + "\\Images";
        public static string standardImage = imagesDir + "\\oneRing.png";

        public static void CopyDBsToDesktop()
        {
            string pather = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\PrzypominajkaDirectory_COPY";
            Directory.CreateDirectory(pather);

            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(DBdirPath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(DBdirPath, pather));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(DBdirPath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(DBdirPath, pather), true);
            }
        }
        public static void ChangeStatusInfo(bool finished)
        {
            if(finished) MainWindow.loadingGif.Visibility = Visibility.Hidden;//MainWindow.testStatus.Text = DefaultStatusText;
            else MainWindow.loadingGif.Visibility = Visibility.Visible;//MainWindow.testStatus.Text = DefaultStatusTextWorking;
        }

        public static void ExtractFileResource(string resource_name, string file_name)
        {
            try
            {
                using (Stream sfile = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource_name))
                {
                    byte[] buf = new byte[sfile.Length];
                    sfile.Read(buf, 0, Convert.ToInt32(sfile.Length));

                    using (FileStream fs = File.Create(file_name))
                    {
                        fs.Write(buf, 0, Convert.ToInt32(sfile.Length));
                        fs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Can't extract resource '{0}' to file '{1}': {2}", resource_name, file_name, ex.Message), ex);
            }
        }

        public static void CreateDesktopShortcut()
        {
            new ShortCutCreator();
        }
    }
}
