using BrandInspector.Models;
using BrandInspector.Presenters.Interfaces;
using BrandInspector.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrandInspector.Views
{
    public partial class MainForm : Form , IMainView
    {
        public string SelectedFilePath {get ; set;}

        public event EventHandler BrowseClicked;
        public event EventHandler ScanFontsClicked;
        public event EventHandler ScanColorsClicked;
        public event EventHandler ScanSizesClicked;
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e) {

            errorsDataGridView.DataSource = _bindingSource;
            _bindingSource.DataSource = new List<ErrorViewModel>();

        private void MainForm_Load(object sender, EventArgs e) { }
      
        private void UpdateStatusBar(int total, int errors)
        {
            lblTotal.Text = $"Total: {total}";
            lblErrors.Text = $"Errors: {errors}";
        }
        public void DisplayResults(IList<TextRunInfo> results)
        {
            throw new NotImplementedException();
        }

        private void BrowseBtn_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "PowerPoint Files (*.pptx)|*.pptx";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    SelectedFilePath = dialog.FileName;
                    filePathTxt.Text = SelectedFilePath;

                    Presenter.OnFileSelected(SelectedFilePath);
                }
            }
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
        private void cancel_Click(object sender, EventArgs e)
        {
            Presenter.CancelProcess();
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {        
            if (e.Node.Tag is ErrorViewModel error)
            {
                HighlightRow(error.RowIndex);
            }
        }

        private void HighlightRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= errorsDataGridView.Rows.Count)
                return;

            errorsDataGridView.ClearSelection();

            var row = errorsDataGridView.Rows[rowIndex];
            row.Selected = true;

            errorsDataGridView.FirstDisplayedScrollingRowIndex = rowIndex;
        }
        private void LoadErrors(List<ErrorViewModel> errors)
        {
            treeErrors.Nodes.Clear();

            var grouped = errors.GroupBy(e => e.SlideNumber);

            foreach (var group in grouped)
            {
                TreeNode parent = new TreeNode("Slide "+group.Key);

                foreach (var error in group)
                {
                    TreeNode child = new TreeNode(
                        $"Slide {error.SlideNumber}: {error.Compliance}"
                    );

                    child.Tag = error; // 🔥 important
                    parent.Nodes.Add(child);
                }

                treeErrors.Nodes.Add(parent);
            }

            treeErrors.ExpandAll();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void StartLoading()
        {
            progressBar.Visible = true;
            progressBar.Style = ProgressBarStyle.Marquee;

            lblStatus.Text = "Scanning...";
        }

        private void StopLoading()
        {
            progressBar.Visible = false;

            lblStatus.Text = "Ready";
        }
    }
}
