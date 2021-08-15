using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Przypominajka_3._0
{
    public class RegistryToggler
    {
        static string registryName = "PrzypominajkaLOTR";
        public static void AutostartEditor()
        {
            bool IsStartupItem()
            {
                RegistryKey Run = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                if (Run.GetValue(registryName) == null)
                    // Jeżeli nie została znaleziona taka wartość w rejestrze
                    return false;
                else
                    // Jeżeli została znaleziona taka wartość w rejestrze
                    return true;
            }
            if (IsStartupItem() == true)
            {
                Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true).DeleteValue(registryName);
                MessageBox.Show("Aplikacja usunięta z AutoStartu");
            }
            else
            {

                //dodawanie
                using (RegistryKey Run = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    // Create data for the Autorun of our program.
                    Run.SetValue(registryName, System.Reflection.Assembly.GetExecutingAssembly().Location + " startup");
                }
                MessageBox.Show("Aplikacja dodana do AutoStartu");
            }
        }
    }
}
