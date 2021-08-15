using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Przypominajka_3._0
{
    class SQLiteReferences
    {
        public static string DBdirPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\PrzypominajkaDirectory";
        public static string DBpath = DBdirPath + "\\PrzypominajkaDB.db";
        public static string imagesDir = DBdirPath + "\\Images";
        public static string standardImage = imagesDir + "\\dupa.jpg";

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
    }

    public enum PrzypominajkaEventType
    {
        Standard,
        Weekly,
        Fortnightly,
        Monthly,
        Annual
    }
}
