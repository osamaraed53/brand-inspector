using BrandInspector.Models;
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


        public void OpenView()
        {
            Application.Run(this);
        }

        public void CloseView()
        {
            Close();
        }



        private void MainForm_Load(object sender, EventArgs e) { }
      

        public void DisplayResults(IList<TextRunInfo> results)
        {
            throw new NotImplementedException();
        }
    }
}
