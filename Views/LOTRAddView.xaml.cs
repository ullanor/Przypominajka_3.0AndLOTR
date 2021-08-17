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
    public partial class LOTRAddView : UserControl
    {
        private bool isIssueLoaded;
        public LOTRAddView()
        {
            InitializeComponent();
            if(LOTR_Manager.LoadedIssueIsModified) { SetEditedIssueData(); }
        }
        private void SetEditedIssueData()
        {
            LoadedLOTR loaded = LOTR_Manager.selectedLOTR;
            IssueGuide.Text = loaded.lguide == string.Empty? "Guide" : loaded.lguide;
            IssuePlay.Text = loaded.lplay == string.Empty ? "Play" : loaded.lplay;
            IssueBattle.Text = loaded.lbattle == string.Empty ? "Battle" : loaded.lbattle;
            IssuePaint.Text = loaded.lpaint == string.Empty ? "Painting" : loaded.lpaint;
            IssueModel.Text = loaded.lmodel == string.Empty ? "Modelling" : loaded.lmodel;
            IssueExtras.Text = loaded.lextras == string.Empty ? "Extras" : loaded.lextras;
            IssueID.Text = loaded.lIssue.ToString();
            isIssueLoaded = true;
        }
        private void ClearFields()
        {
            IssueGuide.Text = "Guide";
            IssuePlay.Text = "Play";
            IssueBattle.Text = "Battle";
            IssuePaint.Text = "Painting";
            IssueModel.Text = "Modelling";
            IssueExtras.Text = "Extras";
            IssueID.Text = "Issue ID";
        }
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string imgSrc = LOTR_Manager.NewIssueImageFinder(isIssueLoaded);
            MainManager.ChangeStatusInfo(false);
            if (imgSrc == string.Empty) return;

            int id = 0;
            try { id = int.Parse(IssueID.Text); }catch(Exception ex) { MessageBox.Show(ex.ToString()); return; }
            if(id < 1 || id > 91) { MessageBox.Show(id+" is not valid issue!"); return; }
            string[] desc6 = { IssueGuide.Text, IssuePlay.Text, IssueBattle.Text, IssuePaint.Text, IssueModel.Text, IssueExtras.Text };
            await Task.Run(() =>
            {
                MainManager.SQL_LOTR.ModifyTableLOTR(id, imgSrc, desc6);
                LOTR_Manager.InitializeLOTRList();
            });
            //ClearFields();
            MessageBox.Show(id + " issue was added!");
            MainManager.ChangeStatusInfo(true);
            MainManager.MainWindow.LoadLOTRviewAfterEdit();//CLOSE USER CONTROL! -> BACK TO MAIN LOTR VIEW
        }
    }
}
