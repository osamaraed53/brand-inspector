using BrandInspector.Constants;
using BrandInspector.Models;
using BrandInspector.Presenters.Interfaces;
using BrandInspector.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace BrandInspector.Views
{
    public partial class MainForm : Form, IMainView
    {
        public IMainPresenter Presenter { get; set; }
        private readonly BindingSource _bindingSource = new BindingSource();

        public string SelectedFilePath { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            errorsDataGridView.DataSource = _bindingSource;
            _bindingSource.DataSource = new List<ErrorViewModel>();
            StopLoading();

        }

        private void UpdateStatusBar(int total, int errors)
        {
            lblTotal.Text = $"Total: {total}";
            lblErrors.Text = $"Errors: {errors}";
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

        private async void FontsBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(filePathTxt.Text))
            {
                MessageBox.Show(ErrorMessages.UnSelectedFile);
                return;
            }

            StartLoading();

            var result = await Presenter.ScanFonts();

            if (result == null)
                return;

            var total = result.Total;
            var errors = result.Errors.Count;
            _bindingSource.DataSource = result.Errors;

            UpdateStatusBar(total, errors);
            LoadErrors(result.Errors);

            StopLoading();

        }
        private async void SizeBtn_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(filePathTxt.Text))
            {
                MessageBox.Show(ErrorMessages.UnSelectedFile);
                return;
            }


            StartLoading();


            var result = await Presenter.ScanSize();


            if (result == null)
                return;

            var total = result.Total;
            var errors = result.Errors.Count;
            _bindingSource.DataSource = result.Errors;

            UpdateStatusBar(total, errors);
            LoadErrors(result.Errors);

            StopLoading();

        }

        private async void ColorsBtn_Click(object sender, EventArgs e)
        {
            StartLoading();

            var result = await Presenter.ScanColors();

            if (result == null)
                return;


            var total = result.Total;
            var errors = result.Errors.Count;
            _bindingSource.DataSource = result.Errors;

            UpdateStatusBar(total, errors);
            LoadErrors(result.Errors);

            StopLoading();


        }


        private void Cancel_Click(object sender, EventArgs e)
        {
            Presenter.CancelProcess();
        }



        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is ErrorViewModel error)
            {
                HighlightRow(error.RowIndex - 1);
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
                TreeNode parent = new TreeNode("Slide " + group.Key);

                foreach (var error in group)
                {
                    TreeNode child = new TreeNode(
                        $"Slide {error.SlideNumber}: {error.Compliance}"
                    )
                    {
                        Tag = error
                    };
                    parent.Nodes.Add(child);
                }

                treeErrors.Nodes.Add(parent);
            }

            treeErrors.ExpandAll();
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
