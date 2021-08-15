using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Przypominajka_3._0
{
    /// <summary>
    /// Interaction logic for StartupMainWindow.xaml
    /// </summary>
    public partial class StartupMainWindow : Window
    {
        public StartupMainWindow()
        {
            InitializeComponent();
            Left = System.Windows.SystemParameters.WorkArea.Width - Width;
            Top = System.Windows.SystemParameters.WorkArea.Height - Height;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MoveToMain_Click(object sender, RoutedEventArgs e)
        {

            MainWindow MainWindowOfP = new MainWindow();
            MainWindowOfP.Show();
            Close();
        }
    }
}
