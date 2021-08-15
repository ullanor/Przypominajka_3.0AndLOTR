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
    }
}
