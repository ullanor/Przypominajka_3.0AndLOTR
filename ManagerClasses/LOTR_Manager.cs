using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Przypominajka_3._0
{
    class LOTR_Manager
    {
        public static bool LoadedIssueIsModified;
        public static LoadedLOTR selectedLOTR;
        public static string NewIssueImageFinder(bool isIssueLoaded)
        {
            string filePath = string.Empty;

            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            if (isIssueLoaded) dlg.InitialDirectory = MainManager.imagesDir;
            dlg.Filter = "Images (*.png *.jpg *.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";
            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                //Copy image
                filePath = Path.Combine(MainManager.imagesDir,dlg.SafeFileName);
                if (File.Exists(filePath)) { MessageBox.Show("This Image is already in the directory!"); return filePath; }
                File.Copy(dlg.FileName, filePath, true);
            }

            return filePath;
        }

        public static void InitializeLOTRList(int row)
        {
            loadedLOTRs.AddRange(MainManager.SQL_LOTR.GetDataFromTableLOTR((row * 10) + 1, (row + 1) * 10));//10 - 19
        }
        public static List<LoadedLOTR> loadedLOTRs;
    }


    public class TableValues
    {
        public string lp { get; set; }
        public BitmapImage value1 { get; set; }
        public BitmapImage value2 { get; set; }
        public BitmapImage value3 { get; set; }
        public BitmapImage value4 { get; set; }
        public BitmapImage value5 { get; set; }
        public BitmapImage value6 { get; set; }
        public BitmapImage value7 { get; set; }
        public BitmapImage value8 { get; set; }
        public BitmapImage value9 { get; set; }
        public BitmapImage value10 { get; set; }
    }

    public class LoadedLOTR
    {
        public int lIssue { get; set; }
        public BitmapImage limgSrc { get; set; }
        public string lguide { get; set; }
        public string lplay { get; set; }
        public string lbattle { get; set; }
        public string lpaint { get; set; }
        public string lmodel { get; set; }
        public string lextras { get; set; }
    }
}
