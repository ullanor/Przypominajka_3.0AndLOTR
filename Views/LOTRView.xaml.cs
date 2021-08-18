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

        private Task<TableValues> FillTable(int i)
        {
            TableValues TV;
            if (i == 9)
            {
                TV = new TableValues
                {
                    lp = i.ToString(),
                    value1 = LOTR_Manager.loadedLOTRs[i * 10].limgSrc,
                };
            }
            else
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
            }
            Dispatcher.Invoke(() =>
            {
                testGrid.Items.Add(TV);
                MainManager.ChangeLoadingText(false, i+1);
            });
            return Task.FromResult(TV);
        }

        private async void OnEventsViewLoaded(object sender, RoutedEventArgs e)
        {
            MainManager.ChangeStatusInfo(false);
            MainManager.ChangeLoadingText(false, 0);
            for (int i = 0; i < 10; i++)
            {
                await Task.Run(() =>
                {
                    FillTable(i);
                });
            }
            MainManager.ChangeLoadingText(true, 0);
            MainManager.ChangeStatusInfo(true);
        }

        private int ColumnNo, RowNo, number;
        private void OnEventSelected(object sender, SelectedCellsChangedEventArgs e)
        {
            ColumnNo = testGrid.CurrentCell.Column.DisplayIndex;
            RowNo = int.Parse((testGrid.CurrentCell.Item as TableValues).lp);
            number = RowNo * 10 + ColumnNo - 1;
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

        private void EditIssueButton_Click(object sender, RoutedEventArgs e)
        {
            MainManager.MainWindow.LoadLOTRaddForEditing();
        }
    }
}
