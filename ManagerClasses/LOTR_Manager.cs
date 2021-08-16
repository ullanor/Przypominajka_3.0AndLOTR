﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Przypominajka_3._0
{
    class LOTR_Manager
    {
        public static string NewIssueImageFinder()
        {
            string filePath = string.Empty;

            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                //Copy image
                filePath = Path.Combine(MainManager.imagesDir,dlg.SafeFileName);
                File.Copy(dlg.FileName, filePath, true);
            }

            return filePath;
        }

        public static void InitializeLOTRList()
        {
            loadedLOTRs = MainManager.SQL_LOTR.GetDataFromTableLOTR();
        }
        public static List<LoadedLOTR> loadedLOTRs;
    }


    public class TableValues
    {
        public string lp { get; set; }
        public string value1 { get; set; }
        public string value2 { get; set; }
        public string value3 { get; set; }
        public string value4 { get; set; }
        public string value5 { get; set; }
        public string value6 { get; set; }
        public string value7 { get; set; }
        public string value8 { get; set; }
        public string value9 { get; set; }
        public string value10 { get; set; }
    }

    public class LoadedLOTR
    {
        public int lIssue { get; set; }
        public string limgSrc { get; set; }
        public string lguide { get; set; }
        public string lplay { get; set; }
        public string lbattle { get; set; }
        public string lpaint { get; set; }
        public string lmodel { get; set; }
        public string lextras { get; set; }
    }
}
