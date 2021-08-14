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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Przypominajka_3._0.Views
{
    /// <summary>
    /// Interaction logic for SecondView.xaml
    /// </summary>
    public partial class EventsAddView : UserControl
    {
        public EventsAddView()
        {
            InitializeComponent();
        }

        private void StartTest_Click(object sender, RoutedEventArgs e)
        {
            string imgSrc = LOTR_Manager.NewIssueImageFinder();
            MainManager.SQL.InsertIntoTableLOTR(45, imgSrc);
            MessageBox.Show(imgSrc);
        }
    }
}
