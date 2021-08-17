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
            LOTR_Manager.selectedLOTR = null;
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
              Dispatcher.Invoke(() =>
              {
                testGrid.Items.Add(TV);
              });
            }
        }

        private async void OnEventsViewLoaded(object sender, RoutedEventArgs e)
        {
            MainManager.ChangeStatusInfo(false);
            await Task.Run(() =>
            {
                FillTable();
            });
            MainManager.ChangeStatusInfo(true);
        }

        private void OnEventSelected(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                int ColumnNo = testGrid.CurrentCell.Column.DisplayIndex;
                int RowNo = int.Parse((testGrid.CurrentCell.Item as TableValues).lp);
                int number = RowNo * 10 + ColumnNo - 1;
                //boxik.Text = RowNo + " col: "+ ColumnNo + " ImgSrc: "+ LOTR_Manager.loadedLOTRs[number].limgSrc;
                LoadedLOTR loaded = LOTR_Manager.loadedLOTRs[number];

                IssueImagePrev.Source = new BitmapImage(new Uri(loaded.limgSrc));
                Guide.Text = $"Guide: {loaded.lguide}";
                Play.Text = $"Play: {loaded.lplay}";
                Battle.Text = $"Battle: {loaded.lbattle}";
                Paint.Text = $"Painting: {loaded.lpaint}";
                Model.Text = $"Modelling: {loaded.lmodel}";
                IssueExtras.Text = $"Extra: {loaded.lextras}";

                LOTR_Manager.selectedLOTR = loaded;
                EditIssueButton.IsEnabled = true;
                IssueNo.Text = loaded.lIssue.ToString();
            }
            catch (Exception) { /*boxik.Text = ex.ToString();*/ }
        }

        private void EditIssueButton_Click(object sender, RoutedEventArgs e)
        {
            MainManager.MainWindow.LoadLOTRaddForEditing();
        }
    }
}
