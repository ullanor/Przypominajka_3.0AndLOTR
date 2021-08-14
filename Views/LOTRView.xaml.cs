using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class LOTRView : UserControl
    {
        //List<LoadedLOTR> loadedLOTR;
        public LOTRView()
        {
            InitializeComponent();
            boxik.Text = MainManager.testStr;
        }

        private void FillTable()
        {
            int numberOfRecords = LOTR_Manager.loadedLOTRs.Count;//91 
            int units = 0;
            if (numberOfRecords % 10 == 0) units = numberOfRecords / 10;
            else units = (numberOfRecords / 10) + 1;
            int rest = Math.Abs(numberOfRecords - units * 10);
            for (int i = 0; i < rest; i++)
                LOTR_Manager.loadedLOTRs.Add(new LoadedLOTR());

            
            TableValues TV;
            for (int i = 0; i < units; i++)
            {
                    TV = new TableValues
                    {
                        lp = i.ToString(),
                        value1 = LOTR_Manager.loadedLOTRs[i * 10].limgSrc,
                        value2 = LOTR_Manager.loadedLOTRs[i * 10 + 1].limgSrc,
                        value3 = LOTR_Manager.loadedLOTRs[i * 10 + 2].limgSrc,
                        value4 = LOTR_Manager.loadedLOTRs[i * 10 + 3].limgSrc,
                        value5 = LOTR_Manager.loadedLOTRs[i * 10 + 4].limgSrc,
                        value6 = LOTR_Manager.loadedLOTRs[i * 10 + 5].limgSrc,
                        value7 = LOTR_Manager.loadedLOTRs[i * 10 + 6].limgSrc,
                        value8 = LOTR_Manager.loadedLOTRs[i * 10 + 7].limgSrc,
                        value9 = LOTR_Manager.loadedLOTRs[i * 10 + 8].limgSrc,
                        value10 = LOTR_Manager.loadedLOTRs[i * 10 + 9].limgSrc,
                    };               
                    this.testGrid.Items.Add(TV);
            }
        }

        private void testGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int ColumnNo = testGrid.CurrentCell.Column.DisplayIndex;
                //var cellInfo = testGrid.SelectedCells[int.Parse(testGrid.CurrentCell.Column.Header.ToString())];
                //string RowNo = ((TableValues)testGrid.SelectedItem).lp;//testGrid.Items.IndexOf(testGrid.SelectedCells[0]);
                int RowNo = int.Parse((testGrid.CurrentCell.Item as TableValues).lp);
                //Image tb = cellInfo.Column.GetCellContent(cellInfo.Item) as Image;
                //TableValues sd = cellInfo.Item as TableValues;
                //MessageBox.Show(tb.Text);
                int number = RowNo*10 + ColumnNo -1;
                boxik.Text = RowNo + " col: "+ ColumnNo + " ImgSrc: "+ LOTR_Manager.loadedLOTRs[number].limgSrc;
            }
            catch (Exception ex) { boxik.Text = ex.ToString(); }
        }

        private void OnEventsViewLoaded(object sender, RoutedEventArgs e)
        {
            //loadedLOTR = MainManager.SQL.GetDataFromTableLOTR();
            FillTable();
            Console.WriteLine("Finished {0:N0} iterations.");
            //FillTable();
            //Thread.Sleep(1000);
           // MainManager.ChangeStatusInfo();
        }

        private void TEdt(object sender, MouseButtonEventArgs e)
        {
            //TableValues selected = (TableValues)testGrid.SelectedItem;
            //MessageBox.Show("DUPA "+selected.);
           // var cellInfo = testGrid.SelectedCells[int.Parse(testGrid.CurrentCell.Column.Header.ToString())];
           // TextBlock tb = cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock;
           // MessageBox.Show(tb.Text);
            //    myText.Text = tb.Text;
        }
    }
}
