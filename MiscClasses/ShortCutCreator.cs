using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Przypominajka_3._0
{
    public class ShortCutCreator
    {
        public ShortCutCreator()
        {
            IWshShell_Class wshShell = new IWshShell_Class();
            string fileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\PrzypominajkaLOTR.lnk";
            IWshShortcut shortcut = (IWshShortcut)wshShell.CreateShortcut(fileName);
            shortcut.TargetPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            shortcut.Save();

            MessageBox.Show("Shortcut created in Desktop "+fileName);
        }
    }
}
