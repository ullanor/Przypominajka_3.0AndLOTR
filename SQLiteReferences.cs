using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Przypominajka_3._0
{
    class SQLiteReferences
    {
        public static string DBdirPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\PrzypominajkaDirectory";
        public static string DBpath = DBdirPath + "\\PrzypominajkaDB.db";
        public static string imagesDir = DBdirPath + "\\Images";
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
